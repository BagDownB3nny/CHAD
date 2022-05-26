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
    public GameObject currentWeapon;
    public List<GameObject> weaponInventory = new List<GameObject>(8);

    private void Awake() {
        playerStatsManager = gameObject.GetComponent<PlayerStatsManager>();
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
            currentWeapon.GetComponent<PlayerRangedWeapon>().Unequip();
        }      
        currentWeapon = Instantiate(weaponInventory[gunIndex], transform.position, Quaternion.identity, transform);
        weaponScript = currentWeapon.GetComponent<PlayerRangedWeapon>();
        weaponScript.holder = gameObject;
    }

    //adds gun to empty slot
    public bool AddGun(GameObject gun) {
        if (weaponInventory.Count < 8) {
            weaponInventory.Add(gun);
            return true;
        }
        return false;
    }

    //discards a gun
    public void DiscardGun(int gunIndex) {
        weaponInventory[gunIndex] = null;
    }
}
