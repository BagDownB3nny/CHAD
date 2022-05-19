using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyMovement : EnemyMovement
{
    //scripts needed
    EnemyStatsManager statsManagerScript;
    EnemyWeaponManager weaponManagerScript;

    [Header("Movement Parameters")]
    public float speed;
    public GameObject target;
    private Vector3 targetSize;
    public Rigidbody2D enemyRb;
    private Vector3 directionVector;
    private float directionRotation;

    private void Awake() {
        //get the statsmanager and ask for the movement stats
        statsManagerScript = gameObject.GetComponent<EnemyStatsManager>();
        statsManagerScript.UpdateMovementStats();
        Debug.Log("ENEMY: transferred movement stats from stats manager to movement");

        weaponManagerScript = gameObject.GetComponent<EnemyWeaponManager>();
        Debug.Log("ENEMY: set reference to weapon manager script in movement");
    }

    void Start()
    {
        FindTarget();
        targetSize = target.GetComponent<SpriteRenderer>().bounds.size;
        enemyRb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        FindTarget();
        if (target != null) {
            Move();
        }
    }

    //moves this enemy towards the player
    protected override void Move(){
        directionVector = target.transform.position - transform.position;
        directionVector.Normalize();
        directionRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;

        Face();
        enemyRb.MovePosition((Vector2) transform.position + ((Vector2) directionVector * speed * Time.deltaTime));
    }

    //flips the sprite to face the target
    protected override void Face() {
        Vector3 distToPlayer = target.transform.position - transform.position;
        if (!(Mathf.Abs(distToPlayer.x) < targetSize.x / 2 && Mathf.Abs(distToPlayer.y) < targetSize.y / 2)) {
            if (directionRotation >= -90 && directionRotation < 89) {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            } else {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    //sets target to a random player and updates the weapon manager of a new target
    protected override void FindTarget() {
        if (target == null) {
            GameObject[] gameObjects;
            gameObjects = GameObject.FindGameObjectsWithTag("Player");
            if (gameObjects.Length > 0) {
                int rand = Random.Range(0, gameObjects.Length - 1);
                target = gameObjects[rand];
            }
            UpdateWeaponTarget();
        }
    }

    public override void UpdateWeaponTarget() {
        weaponManagerScript.SetTarget(target);
    }

    public override void SetStats(float _speed) {
        speed = _speed;
    }
}
