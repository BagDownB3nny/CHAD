using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponManager : MonoBehaviour
{
    //scripts needed
    EnemyStatsManager enemyStatsScript;
    EnemyRangedWeapon weaponScript;

    [Header("Holder Attack Stats")]
    public GameObject target;
    public float attack;
    public float armourPenetration;

    [Header("Enemy Weapon Parameters")]
    public GameObject defaultWeapon;
    public GameObject currentWeapon;

    void Start()
    {
        Debug.Log("equipping enemy weapon");
        EquipWeapon();
    }

    //instantiate a selected gun
    public void EquipWeapon() {      
        currentWeapon = Instantiate(defaultWeapon, transform.position, Quaternion.identity, transform);
        weaponScript = currentWeapon.GetComponent<EnemyRangedWeapon>();
        UpdateWeaponAttackStats();

        Debug.Log("enemy weapon equipped");
    }

    public void SetAttackStats(float _attack, float _armourPenetration) {
        attack = _attack;
        armourPenetration = _armourPenetration;
        UpdateWeaponAttackStats();
    }

    public void SetTarget(GameObject _target) {
        target = _target;
        weaponScript.SetTarget(target);
    }

    public void UpdateWeaponAttackStats() {
        weaponScript.SetAttackStats(target, gameObject, attack, armourPenetration);
    }
}
