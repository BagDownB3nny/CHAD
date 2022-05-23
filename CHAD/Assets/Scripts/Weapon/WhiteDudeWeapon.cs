using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteDudeWeapon : EnemyMeleeWeapon
{
    private GameObject currentDamageDealer;

    private void Start() {
        Attack();
    }

    private void Update() {
        Attack();
    }

    public void Attack() {
        if (timeToNextAttack <= 0 && currentDamageDealer == null) {
            currentDamageDealer = Instantiate(defaultDamageDealer, transform.position, Quaternion.identity, transform);
            currentDamageDealer.GetComponent<DamageDealerStatsManager>().SetStats(holder, holderAttack, holderArmourPenetration,
                    damage, targetType, gameObject);
            timeToNextAttack = attackInterval;
        } else {
            timeToNextAttack -= Time.deltaTime;
        }
    }
}
