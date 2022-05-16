using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float startHealth;
    private float hp;
    public GameObject deathEffect;
    public GameObject damageEffect;

    // Start is called before the first frame update
    void Start()
    {
        hp = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void Die() {
        if (deathEffect != null) {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Camera.main.GetComponent<CameraMotor>().DeclarePlayerDead();

        Destroy(gameObject);
    }
}
