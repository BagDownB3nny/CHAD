using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedGuyWeapon : EnemyRangedWeapon
{

    void Start()
    {
        projectileRotationOffset = -90;
    }
    void Update()
    {
        if (NetworkManager.gameType == GameType.Server) {
            //enemy heading
            FiringDirection();
            //final bullet heading with inaccuracy
            BulletDirection();
            Attack();
        }
    }

    //instantiates an EnemyProjectile towards the current player position
    public void Attack() {
        if (timeToNextShot <= 0) {
            GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
            shot.GetComponent<ProjectileStatsManager>().SetStats(holder, this, gameObject, bulletDirectionVector, projectileRotationOffset);
            timeToNextShot = shotInterval;
        } else {
            timeToNextShot -= Time.deltaTime;
        }
    }
}
