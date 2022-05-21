using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyMovement : EnemyMovement
{
    //scripts needed
    EnemyStatsManager statsManagerScript;
    RangedEnemyWeaponManager weaponManagerScript;

    [Header("Ranged Movement Parameters")]
    public float retreatDistance;
    public float stoppingDistance;

    private void Awake() {
        //get the statsmanager and ask for the movement stats
        statsManagerScript = gameObject.GetComponent<EnemyStatsManager>();
        statsManagerScript.UpdateMovementStats();
        //Debug.Log("ENEMY: transferred movement stats from stats manager to movement");

        weaponManagerScript = gameObject.GetComponent<RangedEnemyWeaponManager>();
        Debug.Log("ENEMY: set reference to weapon manager script in movement");
    }

    private void FixedUpdate() {
        FindTarget();
        if (target != null) {
            Move();
        }
    }

    //enemy moves to player until stopping distance, retreats if player gets closer than retreat distance
    protected override void Move(){
        directionVector = target.transform.position - transform.position;
        float distance = directionVector.magnitude;
        directionVector.Normalize();
        directionRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;

        Face();
        if (distance > stoppingDistance) {
            enemyRb.MovePosition((Vector2) transform.position + ((Vector2) directionVector * speed * Time.deltaTime));
        } else if (distance < retreatDistance) {
            enemyRb.MovePosition((Vector2) transform.position + ((Vector2) directionVector * -speed * Time.deltaTime));
        }
    }

    public override void UpdateWeaponTarget() {
        weaponManagerScript.SetTarget(target);
    }
}
