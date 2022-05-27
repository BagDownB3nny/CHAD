using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    protected Vector3 targetSize;
    public Rigidbody2D enemyRb;
    protected Vector3 directionVector;
    protected float directionRotation;

    void Start()
    {
        targetSize = gameObject.GetComponent<EnemyStatsManager>().target.GetComponent<SpriteRenderer>().bounds.size;
        enemyRb = gameObject.GetComponent<Rigidbody2D>();
    }

    //movement behaviour will be defined by each enemy
    protected abstract void Move();

    public void ReceiveMove(Vector2 _position) {
        transform.position = _position;
    }

    //flips the sprite to face the target
    protected void Face() {
        Vector3 distToPlayer = GetComponent<EnemyStatsManager>().target.transform.position - transform.position;
        if (!(Mathf.Abs(distToPlayer.x) < targetSize.x / 2 && Mathf.Abs(distToPlayer.y) < targetSize.y / 2)) {
            if (directionRotation >= -90 && directionRotation < 89) {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            } else {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
