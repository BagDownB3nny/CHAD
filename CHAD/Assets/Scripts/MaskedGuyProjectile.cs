using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedGuyProjectile : EnemyProjectile
{
    private GameObject player;
    private Vector2 target;
    public float speed;
    public float damage;
    private GameObject origin;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        target = (Vector2) player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if ((Vector2) transform.position == target) {
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

    void DealDamage(GameObject player) {
        if (player.GetComponent<HealthScript>() != null) {
            player.GetComponent<HealthScript>().TakeDamage(damage, gameObject);
        }
    }

    public override void SetOrigin(GameObject origin) {
        this.origin = origin;
    }

    public override void UpdateOriginTargetStatus(bool targetStatus) {
        origin.GetComponent<Enemy>().UpdateTargetStatus(false);
    }
}
