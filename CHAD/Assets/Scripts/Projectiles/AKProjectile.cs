using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKProjectile : RifleProjectile
{
    void Update()
    {
        float distanceTravelled = (transform.position - startLocation).magnitude;

        if (distanceTravelled > range) {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Enemy")) {
            DealDamage(collider.gameObject);
            DestroyProjectile();
        }
    }

    void DestroyProjectile() {
        Destroy(gameObject);
    }

    //deals damage to collided player
    void DealDamage(GameObject target) {
        if (target.GetComponent<HealthScript>() != null) {
            target.GetComponent<HealthScript>().TakeDamage(damage, gameObject);
        }
    }
}
