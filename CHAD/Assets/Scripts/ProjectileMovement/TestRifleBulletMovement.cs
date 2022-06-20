using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRifleBulletMovement : ProjectileMovement
{
    void Start()
    {
        if (NetworkManager.gameType == GameType.Server) {
            gameObject.GetComponent<Rigidbody2D>().velocity = 
                ((Vector2) projectileStatsManager.projectileDirectionVector).normalized * projectileStatsManager.speed;
        }
    }

    //move to targetLocation and destroy if reached
    void FixedUpdate()
    {
        if (NetworkManager.gameType == GameType.Server) {
            float distanceTravelled = (transform.position - projectileStatsManager.originLocationVector).magnitude;
            if (distanceTravelled > projectileStatsManager.range) {
                DestroyProjectile();
            } else {
                SendMove();
            }
        }
    }
}
