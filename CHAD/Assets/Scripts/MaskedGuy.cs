using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedGuy : Enemy
{
    //TODO: implement randomised tracking if there are multiple players in the future
    //player
    private GameObject target;
    private Vector3 playerSize;

    //projectile
    public GameObject projectile;

    //shooting behaviour
    public float shotInterval;
    private float timeToNextShot;

    //true if target is alive or exists
    private bool targetStatus = false;
    public float speed = 1f;
    private Rigidbody2D enemy;
    private Vector2 direction;
    private float heading;

    void Start()
    {
        target = FindTarget();
        playerSize = target.GetComponent<SpriteRenderer>().bounds.size;

        targetStatus = true;

        enemy = GetComponent<Rigidbody2D>();
    }

    //calculates direction vector and heading towards target if possible
    void Update(){
        if (target == null) {
            target = FindTarget();
            targetStatus = true;
        } else {
            direction = target.transform.position - transform.position;
            direction.Normalize();
            heading = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }

    private void FixedUpdate() {
        if (target != null) {
            face(heading);
            track(direction);
            shoot();
        }
    }

    //flips the sprite to face the target
    void face(float heading) {
        Vector3 distToPlayer = target.transform.position - transform.position;
        if (!(Mathf.Abs(distToPlayer.x) < playerSize.x/2 && Mathf.Abs(distToPlayer.y) < playerSize.y/2)) {
            if (heading >= -90 && heading < 89) {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            } else {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        
    }

    //moves this enemy towards the player
    void track(Vector2 direction){
        enemy.MovePosition((Vector2) transform.position + (direction * speed * Time.deltaTime));
    }

    //instantiates an EnemyProjectile towards the current player position
    void shoot() {
        if (timeToNextShot <= 0) {
            GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
            bullet.GetComponent<EnemyProjectile>().SetOrigin(gameObject);
            timeToNextShot = shotInterval;
        } else {
            timeToNextShot -= Time.deltaTime;
        }
    }

    //updates targetStatus
    //usually called by the projectile this enemy fires
    public override void UpdateTargetStatus(bool targetStatus) {
        this.targetStatus = targetStatus;
    }

    //returns a random player if possible
    protected override GameObject FindTarget() {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Player");
        if (gameObjects.Length > 0) {
            int rand = Random.Range(0, gameObjects.Length - 1);
            return gameObjects[rand];
        }
        return null;
    }

    //returns the current target
    public override GameObject GetTarget(){
        return this.target;
    }
}
