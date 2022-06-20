using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeaponManager : MonoBehaviour
{
    //scripts needed
    public EnemyStatsManager statsManagerScript;
    public RangedWeapon rangedWeaponScript;
    public MeleeWeapon meleeWeaponScript;

    [Header("Holder Attack Stats")]
    public float attack;
    public float armourPenetration;

    [Header("Enemy Weapon Parameters")]
    public GameObject defaultWeapon;
    public GameObject currentWeapon;

    private void Awake() {
        statsManagerScript = gameObject.GetComponent<EnemyStatsManager>();
    }

    void Start()
    {
        EquipWeapon();
        if (string.Equals(currentWeapon.tag, "Ranged")) {
            rangedWeaponScript = currentWeapon.GetComponent<RangedWeapon>();
            rangedWeaponScript.holder = gameObject;
        } else if (string.Equals(currentWeapon.tag, "Melee")) {
            meleeWeaponScript = currentWeapon.GetComponent<MeleeWeapon>();
            meleeWeaponScript.holder = gameObject;
        }
    }
    public abstract void EquipWeapon();
}
