using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class CharacterStatsManager : MonoBehaviour
{
    //scripts needed
    public Death deathScript;

    [Header("Reference IDs")]
    public string characterRefId;
    public CharacterType characterType;
    public int localProjectileRefId;
    public int localDamageDealerRefId;

    [Header("Character Stats")]
    public float maxHp;
    public float hp;
    public float attack;
    public float speed;
    public float attackSpeed;
    public float accuracy;
    public float armour;
    public float armourPenetration;
    public float armourEffectiveness;
    public float proficiency;
    public GameObject target;
    public Vector3 targetPosition;
    public GameObject damageEffect;

    [Header("UI Elements")]
    public HealthBar healthBar;
    public EnemyHealthBar enemyHealthBar;

    protected virtual void Awake() {
        deathScript = gameObject.GetComponent<Death>();
        hp = maxHp;
    }

    private void Start() {
        if (enemyHealthBar != null) {
            enemyHealthBar.SetHealth(hp, maxHp);
        }
    }

    public int CalcDamageTaken(float _damageDealt, float _armourPenetration)
    {
        float effectiveArmour = armour * (1 - _armourPenetration);
        float damageTaken = _damageDealt * (1 - effectiveArmour / (effectiveArmour + (1 / armourEffectiveness)));
        return Mathf.RoundToInt(damageTaken);
    }

    public void TakeDamage(float _damageDealt, float _armourPenetration) {
        int damageTaken = CalcDamageTaken(_damageDealt, _armourPenetration);
        hp -= damageTaken;

        if (gameObject.tag == "Player") {
            ServerSend.TakeDamage((int)CharacterType.Player, characterRefId, damageTaken);
        } else if (gameObject.tag == "Enemy") {
            if (characterType == CharacterType.Boss)
            {
                ServerSend.TakeDamage((int)CharacterType.Boss, characterRefId, damageTaken);
            }
            else if (characterType == CharacterType.Enemy)
            {
                ServerSend.TakeDamage((int)CharacterType.Enemy, characterRefId, damageTaken);
            }
        }
        if (damageEffect != null) {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }
        
        if (hp < 0) {
            if (gameObject.tag == "Player") {
                ServerSend.Die((int) CharacterType.Player, characterRefId);
            } else if (gameObject.tag == "Enemy") {
                if (characterType == CharacterType.Boss)
                {
                    ServerSend.Die((int)CharacterType.Boss, characterRefId);
                }
                else if (characterType == CharacterType.Enemy)
                {
                    ServerSend.Die((int)CharacterType.Enemy, characterRefId);
                }
            }
            deathScript.Die();
        }
    }

    public void ReceiveTakeDamage(float _damageTaken)
    {
        hp -= _damageTaken;

        if (enemyHealthBar != null) {
            enemyHealthBar.SetHealth(hp, maxHp);
        }

        if (healthBar != null) {
            healthBar.SetHealth(hp);
        }
        //might want to abstract this to a DamageEffect script
        if (damageEffect != null) {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }
    }

    public void ReceiveDie() {
        deathScript.Die();
    }
}
