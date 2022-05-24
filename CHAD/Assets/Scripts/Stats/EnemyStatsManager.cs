using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : MonoBehaviour, CharacterStatsManager
{
    [Header("Enemy Stats")]
    public int enemyRefId;
    public float hp;
    public float attack;
    public float speed;
    public float armour;
    public float armourPenetration;
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

    public void UpdateMovementStats() {
        movementScript.SetStats(speed);    
    }

    public void UpdateAttackStats() {
        weaponManagerScript.SetAttackStats(attack, armourPenetration);
    }

    public void UpdateTargetStats(GameObject _damager) {
        _damager.GetComponent<Damager>().SetTargetStats(armour, armourEffectiveness);
    }

    public void SetStats(float _hp, float _attack, float _speed, float _armour, 
            float _armourPenetration, float _targetArmourEffectiveness, float _proficiency) {
                hp = _hp;
                attack = _attack;
                speed = _speed;
                armour = _armour;
                armourPenetration = _armourPenetration;
                armourEffectiveness = _targetArmourEffectiveness;
                proficiency = _proficiency;
            }

    public void TakeDamage(float _damageTaken) {
        hp -= _damageTaken;

        //might want to abstract this to a DamageEffect script
        if (damageEffect != null) {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }

        if (hp < 0) {
            deathScript.Die();
        }
    } 
}
