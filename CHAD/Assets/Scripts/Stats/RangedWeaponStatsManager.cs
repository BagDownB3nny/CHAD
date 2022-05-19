using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponStatsManager : MonoBehaviour, WeaponStatsManager
{
    //scripts needed
    CharacterStatsManager holderStatsScript;
    RangedWeapon weaponScript;

    [Header("Weapon Parameters")]
    public float holderAttack;
    public float holderArmourPenetration; 
    public float speed;
    public float damage;
    public float range;
    public GameObject projectile;

    private void Awake() {
        holderStatsScript = gameObject.GetComponent<CharacterStatsManager>();
        weaponScript = gameObject.GetComponent<RangedWeapon>();
    }


    /*
    public void UpdateWeaponStats() {
        weaponScript.SetStats(holderAttack, holderArmourPenetration);
    }
    */

}
