using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyMovement : EnemyMovement
{
    //scripts needed
    EnemyStatsManager statsManagerScript;
    MeleeEnemyWeaponManager weaponManagerScript;

    private void Awake() {
        //get the statsmanager and ask for the movement stats
        statsManagerScript = gameObject.GetComponent<EnemyStatsManager>();
        statsManagerScript.UpdateMovementStats();
        Debug.Log("ENEMY: transferred movement stats from stats manager to movement");

        weaponManagerScript = gameObject.GetComponent<MeleeEnemyWeaponManager>();
        Debug.Log("ENEMY: set reference to weapon manager script in movement");
    }

    private void Update() {
        if (NetworkManager.gameType == GameType.Server) {
            FindTarget();
            if (target != null) {
                Move();
            }
        }
    }

    //enemy keeps moving to the player
    protected override void Move(){
        directionVector = target.transform.position - transform.position;
        directionVector.Normalize();
        directionRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;

        Face();
        enemyRb.MovePosition((Vector2) transform.position + ((Vector2) directionVector * speed * Time.deltaTime));

        //send position to client
        ServerSend.MoveEnemy(statsManagerScript.characterRefId, transform.position);
    }

    public override void UpdateWeaponTarget() {
        //DO NOTHING SINCE MELEE NO TARGET
        //weaponManagerScript.SetTarget(target);
    }
}
