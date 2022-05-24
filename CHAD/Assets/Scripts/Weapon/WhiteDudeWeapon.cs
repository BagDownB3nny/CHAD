using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteDudeWeapon : EnemyMeleeWeapon
{
    private GameObject currentDamageDealer;

    private void Start() {
        
    }

    private void Update() {
        Attack();
    }

    public void Attack() {
        if (timeToNextAttack <= 0 && currentDamageDealer == null) {
            currentDamageDealer = Instantiate(defaultDamageDealer, transform.position, Quaternion.identity, transform);
            currentDamageDealer.GetComponent<DamageDealerStatsManager>().SetStats(holder, 
                holder.GetComponent<EnemyStatsManager>().attack, holder.GetComponent<EnemyStatsManager>().armourPenetration,
                    damage, targetType, gameObject);
            timeToNextAttack = attackInterval;
        } else {
            timeToNextAttack -= Time.deltaTime;
        }
    }
}
