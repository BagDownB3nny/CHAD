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
    private float rotationOffset = -90;


    //gets the target from the origin and calculates targetLocation
    void Start()
    {
        target = origin.GetComponent<Enemy>().GetTarget();
        targetLocation = (Vector2) target.transform.position;

        Vector3 direction = (Vector3) targetLocation - transform.position;
        direction.Normalize();
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation + rotationOffset);
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
    void DealDamage(GameObject target) {
        if (target.GetComponent<HealthScript>() != null) {
            target.GetComponent<HealthScript>().TakeDamage(damage, gameObject);
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
