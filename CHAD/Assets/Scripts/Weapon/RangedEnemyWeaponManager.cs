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
        weaponScript = currentWeapon.GetComponent<EnemyRangedWeapon>();
        weaponScript.holder = gameObject;
    }

    public void SetTarget(GameObject _target) {
        target = _target;
        weaponScript.SetTarget(target);
    }
}
