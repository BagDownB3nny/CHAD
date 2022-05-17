using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthScript : HealthScript
{
    // Start is called before the first frame update
    public float startHealth;
    private float hp;
    public GameObject deathEffect;
    public GameObject damageEffect;

    void Start()
    {
        hp = startHealth;
    }

    public override void TakeDamage(float damage, GameObject bullet) {
        hp -= damage;
        Debug.Log(hp);
        if (damageEffect != null) {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }

        if (hp <= 0) {
            Die();
        }
    }

    //instantiate deathEffect, update camera and targeting enemy of death
    public override void Die() {
        if (deathEffect != null) {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
