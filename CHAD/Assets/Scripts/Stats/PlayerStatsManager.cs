using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    [Header("scripts collected")]
    //Scripts needed
    public Death deathScipt;

    [Header("Player Stats")]
    public float hp;
    public float speed;
    public float armour;
    public float armourEffectiveness;
    public float proficiency;
    public GameObject damageEffect;

    [Header("Network Id")]
    public int playerId;

    private void Awake() {
        deathScipt = gameObject.GetComponent<Death>();
    }

    public override void TakeDamage(float _damageDealt, float _armourPenetration) {
        float effectiveArmour = armour * (1 - armourPenetration);
        float damageTaken = _damageDealt * (1 - effectiveArmour/(effectiveArmour + (1/armourEffectiveness)));
        hp -= damageTaken;

        if (damageEffect != null) {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }
        
        if (hp < 0) {
            deathScipt.Die();
        }
    }
}
