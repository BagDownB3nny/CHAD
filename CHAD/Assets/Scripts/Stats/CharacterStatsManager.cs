using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStatsManager : MonoBehaviour
{
    //scripts needed
    public Death deathScript;

    [Header("Character Reference ID")]
    public string characterRefId;

    [Header("Character Stats")]
    public float hp;
    public float attack;
    public float speed;
    public float armour;
    public float armourPenetration;
    public float armourEffectiveness;
    public float proficiency;
    public GameObject damageEffect;

    protected virtual void Awake() {
        deathScript = gameObject.GetComponent<Death>();
    }

    public abstract void UpdateMovementStats();
    public abstract void UpdateAttackStats();
    public abstract void UpdateTargetStats(GameObject _damager);

    public override void TakeDamage(float _damageDealt, float _armourPenetration) {
        float effectiveArmour = armour * (1 - armourPenetration);
        float damageTaken = _damageDealt * (1 - effectiveArmour/(effectiveArmour + (1/armourEffectiveness)));
        hp -= damageTaken;

        if (gameObject.tag == "Player") {
            ServerSend.TakeDamage((int) CharacterType.Player, characterRefId, _damageTaken);
        } else if (gameObject.tag == "Enemy") {
            ServerSend.TakeDamage((int) CharacterType.Enemy, characterRefId, _damageTaken);
        }

        if (damageEffect != null) {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }
        
        if (hp < 0) {
            if (gameObject.tag == "Player") {
                ServerSend.Die((int) CharacterType.Player, characterRefId);
            } else if (gameObject.tag == "Enemy") {
                ServerSend.Die((int) CharacterType.Enemy, characterRefId);
            }
            deathScript.Die();
        }
    }

    public void ReceiveTakeDamage(float _damageTaken)
    {
        hp -= _damageTaken;

        //might want to abstract this to a DamageEffect script
        if (damageEffect != null) {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }
    }

    public void ReceiveDie() {
        deathScript.Die();
    }
}
