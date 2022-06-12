using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    //scripts needed
    public PlayerStatsManager playerStatsManager;
    public PlayerRangedWeapon weaponScript;

    [Header("Player Weapons Parameters")]
    public GameObject defaultWeapon;
    public GameObject defaultWeapon2;
    public GameObject currentWeapon;
    public Dictionary<int, GameObject> weaponInventory = new Dictionary<int, GameObject>(8);

    private void Awake() {
        playerStatsManager = gameObject.GetComponent<PlayerStatsManager>();
    }
    
    void Start()
    {
        //AddGun(defaultGun);
        AddGun(defaultWeapon);
        AddGun(defaultWeapon2);
        if (NetworkManager.IsMine(playerStatsManager.characterRefId)) {
             EquipGun(0);
        }
    }

    

    //instantiate a selected gun
    public void EquipGun(int gunIndex) {
        //if currently holding a gun, discard it first
        if (!weaponInventory.ContainsKey(gunIndex)) {
            return;
        }
        if (currentWeapon != null) {
            currentWeapon.GetComponent<PlayerRangedWeapon>().Unequip();
        }      
        currentWeapon = Instantiate(weaponInventory[gunIndex], transform.position, Quaternion.identity, transform);
        weaponScript = currentWeapon.GetComponent<PlayerRangedWeapon>();
        weaponScript.holder = gameObject;

        GameUIManager.instance.SetWeaponIcon(currentWeapon.GetComponent<SpriteRenderer>().sprite);
        ClientSend.EquipGun(gunIndex);
    }

    public void ReceiveEquipGun(int gunIndex) {
        if (currentWeapon != null) {
            currentWeapon.GetComponent<PlayerRangedWeapon>().Unequip();
        }      
        currentWeapon = Instantiate(weaponInventory[gunIndex], transform.position, Quaternion.identity, transform);
        weaponScript = currentWeapon.GetComponent<PlayerRangedWeapon>();
        weaponScript.holder = gameObject;
    }

    //adds gun to empty slot
    public bool AddGun(GameObject gun) {
        if (weaponInventory.Count < 8) {
            weaponInventory.Add(weaponInventory.Count, gun);
            return true;
        }
        return false;
    }

    //discards a gun
    public void DiscardGun(int gunIndex) {
        weaponInventory[gunIndex] = null;
    }
}
