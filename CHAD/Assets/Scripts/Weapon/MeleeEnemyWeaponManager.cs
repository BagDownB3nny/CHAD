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
        Debug.Log("Instantiated Enemy Weapon");
        weaponScript = currentWeapon.GetComponent<EnemyMeleeWeapon>();
        Debug.Log("ENEMY: weaponscript reference created");
        UpdateWeaponAttackStats();

        Debug.Log("enemy weapon equipped");
    }

    public override void UpdateWeaponAttackStats() {
        weaponScript.SetAttackStats(gameObject, attack, armourPenetration);
    }
}
