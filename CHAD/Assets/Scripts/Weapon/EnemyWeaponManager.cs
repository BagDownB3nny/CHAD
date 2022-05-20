using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeaponManager : MonoBehaviour
{
    //scripts needed
    EnemyStatsManager statsManagerScript;

    [Header("Holder Attack Stats")]
    public float attack;
    public float armourPenetration;

    [Header("Enemy Weapon Parameters")]
    public GameObject defaultWeapon;
    public GameObject currentWeapon;

    private void Awake() {
        statsManagerScript = gameObject.GetComponent<EnemyStatsManager>();
        statsManagerScript.UpdateAttackStats();
        Debug.Log("transferred attack stats from stats manager to weapon manager");
    }

    void Start()
    {
        Debug.Log("equipping enemy weapon");
        EquipWeapon();
    }

    public abstract void EquipWeapon();

    public void SetAttackStats(float _attack, float _armourPenetration) {
        attack = _attack;
        armourPenetration = _armourPenetration;
        //only relay the updates to the weapon script if there is a weapon
        if (currentWeapon != null) {
            UpdateWeaponAttackStats();
        }
    }

    public abstract void UpdateWeaponAttackStats();

    /*
    //scripts needed
    EnemyStatsManager statsManagerScript;
    EnemyRangedWeapon weaponScript;

    [Header("Holder Attack Stats")]
    public GameObject target;
    public float attack;
    public float armourPenetration;

    [Header("Enemy Weapon Parameters")]
    public GameObject defaultWeapon;
    public GameObject currentWeapon;

    private void Awake() {
        statsManagerScript = gameObject.GetComponent<EnemyStatsManager>();
        statsManagerScript.UpdateAttackStats();
        Debug.Log("transferred attack stats from stats manager to weapon manager");
    }

    void Start()
    {
        Debug.Log("equipping enemy weapon");
        EquipWeapon();
    }

    //instantiate a selected gun
    public void EquipWeapon() {      
        currentWeapon = Instantiate(defaultWeapon, transform.position, Quaternion.identity, transform);
        Debug.Log("Instantiated Enemy Weapon");
        weaponScript = currentWeapon.GetComponent<EnemyRangedWeapon>();
        Debug.Log("ENEMY: weaponscript reference created");
        UpdateWeaponAttackStats();

        Debug.Log("enemy weapon equipped");
    }

    public void SetAttackStats(float _attack, float _armourPenetration) {
        attack = _attack;
        armourPenetration = _armourPenetration;
        //only relay the updates to the weapon script if there is a weapon
        if (weaponScript != null) {
            UpdateWeaponAttackStats();
        }
    }

    public void SetTarget(GameObject _target) {
        target = _target;
        weaponScript.SetTarget(target);
    }

    public void UpdateWeaponAttackStats() {
        weaponScript.SetAttackStats(gameObject, attack, armourPenetration);
    }
    */
}
