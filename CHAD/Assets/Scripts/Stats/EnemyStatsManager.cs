using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : CharacterStatsManager
{
    [Header("Enemy Stats")]
    public int enemyRefId;
    public float hp;
    public float speed;
    public float armour;
    public float armourEffectiveness;
    public float proficiency;
    public GameObject damageEffect;

    //Scripts needed
    EnemyMovement movementScript;
    EnemyWeaponManager weaponManagerScript;
    Death deathScript;

    private void Awake() {
        movementScript = gameObject.GetComponent<EnemyMovement>();
        weaponManagerScript = gameObject.GetComponent<EnemyWeaponManager>();
        deathScript = gameObject.GetComponent<Death>();
    }

    public override void TakeDamage(float _damageDealt, float _armourPenetration) {
        float effectiveArmour = armour * (1 - _armourPenetration);
        float damageTaken = _damageDealt * (1 - effectiveArmour/(effectiveArmour + (1/armourEffectiveness)));
        hp -= damageTaken;

        //might want to abstract this to a DamageEffect script
        if (damageEffect != null) {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }

        if (hp < 0) {
            deathScript.Die();
        }
    } 
}
