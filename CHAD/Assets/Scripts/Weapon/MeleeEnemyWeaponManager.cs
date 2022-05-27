using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyWeaponManager : EnemyWeaponManager
{
    //scripts needed
    EnemyMeleeWeapon weaponScript;


    //instantiate a selected gun
    public override void EquipWeapon() {      
        currentWeapon = Instantiate(defaultWeapon, transform.position, Quaternion.identity, transform);
        weaponScript = currentWeapon.GetComponent<EnemyMeleeWeapon>();
        weaponScript.holder = gameObject;
    }
}
