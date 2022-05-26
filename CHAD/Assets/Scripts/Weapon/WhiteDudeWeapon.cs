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
            // Debug.Log("Holder: " + holder);
            // Debug.Log("DamageDealerStatsManager: " + currentDamageDealer.GetComponent<DamageDealerStatsManager>());
            // Debug.Log("Attack: " + holder.GetComponent<EnemyStatsManager>().attack);
            // Debug.Log("Arm pen: " + holder.GetComponent<EnemyStatsManager>().armourPenetration);
            // Debug.Log("Damage: "+ damage);
            // Debug.Log("TargetType: " + targetType);

            EnemyStatsManager holderScript = holder.GetComponent<EnemyStatsManager>();
            Debug.Log(holderScript != null);

            currentDamageDealer.GetComponent<DamageDealerStatsManager>().SetStats(holder, 
                holder.GetComponent<EnemyStatsManager>().attack, holder.GetComponent<EnemyStatsManager>().armourPenetration,
                    damage, targetType, gameObject);
            timeToNextAttack = attackInterval;
        } else {
            timeToNextAttack -= Time.deltaTime;
        }
    }
}
