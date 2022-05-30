using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyWeaponManager : EnemyWeaponManager
{
    //instantiate a selected gun
    public override void EquipWeapon() {      
        currentWeapon = Instantiate(defaultWeapon, transform.position, Quaternion.identity, transform);
    }
}
