using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    //scripts needed
    public PlayerStatsManager statsManagerScript;
    public PlayerRangedWeapon weaponScript;

    [Header("Holder Attack Stats")]
    public float attack;
    public float armourPenetration;

    [Header("Player Weapons Parameters")]
    public GameObject defaultWeapon;
    public GameObject currentWeapon;
    public List<GameObject> weaponInventory = new List<GameObject>(8);

    private void Awake() {
        statsManagerScript = gameObject.GetComponent<PlayerStatsManager>();
        statsManagerScript.UpdateAttackStats();
        Debug.Log("PLAYER: transferred attack stats from stats manager to weapon manager");
        
    }
    
    void Start()
    {
        //AddGun(defaultGun);
        weaponInventory.Add(defaultWeapon);
        EquipGun(0);
    }

    //instantiate a selected gun
    public void EquipGun(int gunIndex) {
        //if currently holding a gun, discard it first
        if (currentWeapon != null) {
            currentWeapon.GetComponent<PlayerRangedWeapon>().Discard();
        }      
        currentWeapon = Instantiate(weaponInventory[gunIndex], transform.position, Quaternion.identity, transform);
        weaponScript = currentWeapon.GetComponent<PlayerRangedWeapon>();
        UpdateWeaponAttackStats();
    }

    //adds gun to empty slot
    public bool AddGun(GameObject gun) {
        if (weaponInventory.Count < 8) {
            weaponInventory.Add(gun);
            return true;
        }
        return false;
    }

    public void SetAttackStats(float _attack, float _armourPenetration) {
        attack = _attack;
        armourPenetration = _armourPenetration;
        //only relay the updates to the weapon script if there is a weapon
        if (currentWeapon != null) {
            UpdateWeaponAttackStats();
        }
    }

    public void UpdateWeaponAttackStats() {
        currentWeapon.GetComponent<PlayerRangedWeapon>().SetAttackStats(gameObject, attack, armourPenetration);
    }

    //discards a gun
    public void DiscardGun(int gunIndex) {
        weaponInventory[gunIndex] = null;
    }
}
