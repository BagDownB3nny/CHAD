using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float startHealth;
    private float hp;
    public GameObject deathEffect;
    public GameObject damageEffect;

    void Start()
    {
        hp = startHealth;
    }

    public void TakeDamage(float damage, GameObject bullet) {
        hp -= damage;
        Debug.Log(hp);
        if (damageEffect != null) {
            Instantiate(damageEffect, transform.position, Quaternion.identity);
        }

        if (hp <= 0) {
            bullet.GetComponent<EnemyProjectile>().UpdateOriginTargetStatus(false);
            Die();
        }
    }

    //instantiate deathEffect, update camera and targeting enemy of death
    private void Die() {
        if (deathEffect != null) {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        
        Camera.main.GetComponent<CameraMotor>().DeclarePlayerDead();
        Destroy(gameObject);
    }
}
