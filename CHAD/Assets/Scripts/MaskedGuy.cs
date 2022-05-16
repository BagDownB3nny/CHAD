using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedGuy : Enemy
{
    //TODO: implement randomised tracking if there are multiple players in the future
    //player
    private GameObject player;
    private Vector3 playerSize;

    //projectile
    public GameObject projectile;

    //shooting behaviour
    public float shotInterval;
    private float timeToNextShot;

    private bool hasTarget = false;
    public float speed = 1f;
    private Rigidbody2D enemy;
    private Vector2 direction;
    private float heading;

    void Start()
    {
        player = GameObject.Find("Player");
        playerSize = player.GetComponent<SpriteRenderer>().bounds.size;

        hasTarget = true;

        enemy = GetComponent<Rigidbody2D>();
    }

    void Update(){
        if (!hasTarget) {
            player = GameObject.Find("Player");
        }
        if (player != null) {
            direction = player.transform.position - transform.position;
            direction.Normalize();
            heading = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }

    private void FixedUpdate() {
        if (player != null) {
            face(heading);
            track(direction);
            shoot();
        }
    }
   
    void face(float heading) {
        Vector3 distToPlayer = player.transform.position - transform.position;
        if (!(Mathf.Abs(distToPlayer.x) < playerSize.x/2 && Mathf.Abs(distToPlayer.y) < playerSize.y/2)) {
            if (heading >= -90 && heading < 89) {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            } else {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        
    }

    void track(Vector2 direction){
        enemy.MovePosition((Vector2) transform.position + (direction * speed * Time.deltaTime));
    }

    void shoot() {
        if (timeToNextShot <= 0) {
            GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
            bullet.GetComponent<EnemyProjectile>().SetOrigin(gameObject);
            timeToNextShot = shotInterval;
        } else {
            timeToNextShot -= Time.deltaTime;
        }
    }

    public override void UpdateTargetStatus(bool targetStatus) {
        hasTarget = targetStatus;
    }
}
