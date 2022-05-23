using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedGuyWeapon : EnemyRangedWeapon
{
    void Update()
    {
        //enemy heading
        FiringDirection();
        //final bullet heading with inaccuracy
        BulletDirection();
        Attack();
    }

    //instantiates an EnemyProjectile towards the current player position
    public void Attack() {
        if (timeToNextShot <= 0) {
            GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
            shot.GetComponent<ProjectileStatsManager>().SetStats(holder, holderAttack, holderArmourPenetration, speed, 
                    damage, range, targetType, gameObject, transform.position, bulletDirectionVector, projectileRotationOffset);
            timeToNextShot = shotInterval;
        } else {
            timeToNextShot -= Time.deltaTime;
        }
    }
}
