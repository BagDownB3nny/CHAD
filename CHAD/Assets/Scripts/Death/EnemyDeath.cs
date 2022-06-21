using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour, Death
{
    public delegate void OnEnemyDeath(GameObject enemy);
    public static OnEnemyDeath onEnemyDeath;
    public GameObject deathEffect;

    public void Die() {
        onEnemyDeath(gameObject);
        if (deathEffect != null) {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
