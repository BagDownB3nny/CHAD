using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    //scripts needed
    public PlayerStatsManager playerStatsManager;
    public PlayerRangedWeapon weaponScript;

    [Header("Player Weapons Parameters")]
    public GameObject currentWeapon;
    public int currentWeaponId;
    public Dictionary<int, PlayerWeapons> weaponInventory = new Dictionary<int, PlayerWeapons>(8);

    private void Awake() {
        playerStatsManager = gameObject.GetComponent<PlayerStatsManager>();
    }
    
    void Start()
    {
        if (NetworkManager.IsMine(playerStatsManager.characterRefId)) {
            EquipGun(0);
        }
    }

    public void SetWeaponInventory(PlayerInfo playerInfo)
    {
        if (NetworkManager.IsMine(GetComponent<PlayerStatsManager>().characterRefId))
        {
            GameUIManager.instance.weaponWheel.GetComponent<WeaponWheel>().ResetWheel();
        }
        foreach (PlayerWeapons gun in playerInfo.weaponInventory.Values)
        {
            AddGun(gun);
        }
    }

    private void OnDestroy()
    {
        PlayerInfoManager.AllPlayerInfo[GetComponent<PlayerStatsManager>().characterRefId].SetWeaponInventory(this);
    }


    //instantiate a selected gun
    public void EquipGun(int gunIndex) {
        SoundManager.instance.PlaySound(Sounds.EquipGun);
        //if currently holding a gun, discard it first
        if (!weaponInventory.ContainsKey(gunIndex)) {
            return;
        }
        currentWeaponId = gunIndex;
        if (currentWeapon != null) {
            currentWeapon.GetComponent<PlayerRangedWeapon>().Unequip();
        }      
        currentWeapon = Instantiate(GameManager.instance.gunPrefabs[(int)weaponInventory[gunIndex]], transform.position, Quaternion.identity, transform);
        weaponScript = currentWeapon.GetComponent<PlayerRangedWeapon>();
        weaponScript.holder = gameObject;

        GameUIManager.instance.SetWeaponIcon(currentWeapon.GetComponent<SpriteRenderer>().sprite);
        ClientSend.EquipGun(gunIndex);
    }

    public void ReceiveEquipGun(int gunIndex) {
        currentWeaponId = gunIndex;
        if (currentWeapon != null) {
            currentWeapon.GetComponent<PlayerRangedWeapon>().Unequip();
        }      
        currentWeapon = Instantiate(GameManager.instance.gunPrefabs[(int)weaponInventory[gunIndex]], transform.position, Quaternion.identity, transform);
        weaponScript = currentWeapon.GetComponent<PlayerRangedWeapon>();
        weaponScript.holder = gameObject;
    }

    //adds gun to empty slot
    public bool AddGun(PlayerWeapons gunType) {
        if (weaponInventory.Count < 8 ) {
            GameObject gun = GameManager.instance.gunPrefabs[(int) gunType];
            weaponInventory.Add(weaponInventory.Count, gunType);
            if (NetworkManager.IsMine(GetComponent<PlayerStatsManager>().characterRefId)) {
                SoundManager.instance.PlaySound(Sounds.Interact);
                GameUIManager.instance.weaponWheel.GetComponent<WeaponWheel>()
                    .UpdateWeaponButton(weaponInventory.Count, gun);
            }
            return true;
        }
        return false;
    }

    //discards a gun
    public void DiscardGun(int gunIndex) {
        weaponInventory.Remove(gunIndex);
        // TODO: Drop a weaponDrop
    }
}
