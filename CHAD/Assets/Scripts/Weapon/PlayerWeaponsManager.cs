using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{
    //scripts needed
    PlayerStatsManager playerStatsScript;

    [Header("Holder Attack Stats")]
    public float attack;
    public float armourPenetration;

    [Header("Player Weapons Parameters")]
    public GameObject defaultWeapon;
    public GameObject currentWeapon;
    public List<GameObject> weaponInventory = new List<GameObject>(8);

    void Start()
    {
        //AddGun(defaultGun);
        weaponInventory.Add(defaultWeapon);
        Debug.Log("equipping gun");
        EquipGun(0);
    }

    //instantiate a selected gun
    public void EquipGun(int gunIndex) {
        //if currently holding a gun, discard it first
        if (currentWeapon != null) {
            currentWeapon.GetComponent<PlayerRangedWeapon>().Discard();
            Debug.Log("weapon discarded");
        }      
        currentWeapon = Instantiate(weaponInventory[gunIndex], transform.position, Quaternion.identity, transform);
        UpdateWeaponAttackStats();

        Debug.Log("equipped" + gunIndex);
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
        UpdateWeaponAttackStats();
    }

    public void UpdateWeaponAttackStats() {
        currentWeapon.GetComponent<PlayerRangedWeapon>().SetAttackStats(gameObject, attack, armourPenetration);
    }

    //discards a gun
    public void DiscardGun(int gunIndex) {
        weaponInventory[gunIndex] = null;
    }
}
