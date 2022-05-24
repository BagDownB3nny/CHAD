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
    }

    private void Update() {
        if (NetworkManager.gameType == GameType.Server) {
            FindTarget();
            if (target != null) {
                Move();
            }
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
            enemyRb.MovePosition((Vector2) transform.position + ((Vector2) directionVector * statsManagerScript.speed * Time.deltaTime));
        } else if (distance < retreatDistance) {
            enemyRb.MovePosition((Vector2) transform.position + ((Vector2) directionVector * -statsManagerScript.speed * Time.deltaTime));
        }

        //send position to client
        ServerSend.MoveEnemy(statsManagerScript.enemyRefId, transform.position);
    }

    public override void UpdateWeaponTarget() {
        weaponManagerScript.SetTarget(target);
    }
}
