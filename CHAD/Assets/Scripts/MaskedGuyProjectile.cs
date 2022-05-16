using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedGuyProjectile : EnemyProjectile
{
    private GameObject target;
    private Vector2 targetLocation;
    public float speed;
    public float damage;
    //the GameObject that produced this Projectile
    private GameObject origin;


    //gets the target from the origin and calculates targetLocation
    void Start()
    {
        target = origin.GetComponent<Enemy>().GetTarget();
        targetLocation = (Vector2) target.transform.position;
    }

    //move to targetLocation and destroy if reached
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetLocation, speed * Time.deltaTime);

        if ((Vector2) transform.position == targetLocation) {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            DealDamage(collider.gameObject);
            DestroyProjectile();
        }
    }

    void DestroyProjectile() {
        Destroy(gameObject);
    }

    //deals damage to collided player
    void DealDamage(GameObject player) {
        if (player.GetComponent<HealthScript>() != null) {
            player.GetComponent<HealthScript>().TakeDamage(damage, gameObject);

        }
    }

    public override void SetOrigin(GameObject origin) {
        this.origin = origin;
    }

    //updates the origin on targetStatus
    public override void UpdateOriginTargetStatus(bool targetStatus) {
        origin.GetComponent<Enemy>().UpdateTargetStatus(false);
    }
}
