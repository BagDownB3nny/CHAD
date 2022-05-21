using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyWeaponManager : EnemyWeaponManager
{
    //scripts needed
    EnemyRangedWeapon weaponScript;

    public GameObject target;

    //instantiate a selected gun
    public override void EquipWeapon() {      
        currentWeapon = Instantiate(defaultWeapon, transform.position, Quaternion.identity, transform);
        Debug.Log("Instantiated Enemy Weapon");
        weaponScript = currentWeapon.GetComponent<EnemyRangedWeapon>();
        Debug.Log("ENEMY: weaponscript reference created");
        UpdateWeaponAttackStats();

        Debug.Log("enemy weapon equipped");
    }

    public void SetTarget(GameObject _target) {
        target = _target;
        weaponScript.SetTarget(target);
    }

    public override void UpdateWeaponAttackStats() {
        weaponScript.SetAttackStats(gameObject, attack, armourPenetration);
    }
}
