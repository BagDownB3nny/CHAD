using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    [Header("Weapon Parameters")]
    public GameObject defaultDamageDealer;
    public GameObject currentDamageDealer;

    public override void Attack() {
        CharacterStatsManager characterStats = holder.GetComponent<CharacterStatsManager>();
        currentDamageDealer = Instantiate(defaultDamageDealer, transform.position, Quaternion.identity, transform);
        string damageDealerRefId = string.Format("{0}.{1}", characterStats.characterRefId, 
                    characterStats.localDamageDealerRefId);
        characterStats.localDamageDealerRefId++;
        currentDamageDealer.GetComponent<DamageDealerStatsManager>().SetStats(damageDealerRefId, holder, 
            holder.GetComponent<EnemyStatsManager>().attack, holder.GetComponent<EnemyStatsManager>().armourPenetration,
                damage, targetType, gameObject);
        GameManager.instance.damageDealers.Add(damageDealerRefId, currentDamageDealer);
        ServerSend.MeleeAttack(characterStats.characterType, characterStats.characterRefId, damageDealerRefId);
        timeToNextAttack = attackInterval;
    }

    public void ReceiveAttack(string _damageDealerRefId) {
        currentDamageDealer = Instantiate(defaultDamageDealer, transform.position, Quaternion.identity, transform);
        currentDamageDealer.GetComponent<DamageDealerStatsManager>().SetStats(_damageDealerRefId, holder, 
            holder.GetComponent<EnemyStatsManager>().attack, holder.GetComponent<EnemyStatsManager>().armourPenetration,
                damage, targetType, gameObject);
        GameManager.instance.damageDealers.Add(_damageDealerRefId, currentDamageDealer);
    }

}
