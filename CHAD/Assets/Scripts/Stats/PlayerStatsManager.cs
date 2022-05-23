using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour, CharacterStatsManager
{
    [Header("scripts collected")]
    //Scripts needed
    public PlayerMovement movementScript;
    public PlayerWeaponsManager weaponsManagerScipt;
    public Death deathScipt;

    [Header("Player Stats")]
    public float hp;
    public float attack;
    public float speed;
    public float armour;
    public float armourPenetration;
    public float armourEffectiveness;
    public float proficiency;
    public GameObject damageEffect;

    [Header("Network Id")]
    public int myId;

    private void Awake() {
        movementScript = gameObject.GetComponent<PlayerMovement>();
        weaponsManagerScipt = gameObject.GetComponent<PlayerWeaponsManager>();
        deathScipt = gameObject.GetComponent<Death>();
    }

    public void UpdateMovementStats() {
        movementScript.SetMovementStats(speed);    
    }

    //call whenever there is a change in attack stats
    public void UpdateAttackStats() {
        //transfer attack stats to weapon manager, then to weapon then to projectile
        weaponsManagerScipt.SetAttackStats(attack, armourPenetration);
    }

    public void UpdateTargetStats(GameObject _damager) {
        //can be cany type of damager doesnt matter as long as this script passes the info over
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
        //Debug.Log("Took Damage, HP: " + hp);

        //might want to abstract this to a DamageEffect script
        if (damageEffect != null) {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }
        
        if (hp < 0) {
            deathScipt.Die();
        }
    }
}
