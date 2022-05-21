using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float speed;
    public GameObject target;
    protected Vector3 targetSize;
    public Rigidbody2D enemyRb;
    protected Vector3 directionVector;
    protected float directionRotation;

    void Start()
    {
        FindTarget();
        targetSize = target.GetComponent<SpriteRenderer>().bounds.size;
        enemyRb = gameObject.GetComponent<Rigidbody2D>();
    }

    //movement behaviour will be defined by each enemy
    protected abstract void Move();

    //flips the sprite to face the target
    protected void Face() {
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
    protected void FindTarget() {
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

    public void SetStats(float _speed) {
        speed = _speed;
    }

    //might not be needed since melee enemymovement doesnt need target information for weapon
    public abstract void UpdateWeaponTarget();
}
