using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRifle : PlayerRangedWeapon
{
    void Update()
    {
        //player heading
        FiringDirection();
        //final bullet heading with inaccuracy
        BulletDirection();
        PointAtMouse();
        if (Input.GetMouseButton(0)){
            Attack();
        }
    }

    public override void Attack() {
        if (timeToNextShot <= 0) {
            GameObject shot = Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, directionRotation + projectileRotationOffset));
            shot.GetComponent<ProjectileStatsManager>().SetStats(holder, holderAttack, holderArmourPenetration, speed, 
                    damage, range, targetType, gameObject, transform.position, bulletDirectionVector, projectileRotationOffset);
            timeToNextShot = shotInterval;
        } else {
            timeToNextShot -= Time.deltaTime;
        }
    }    
}
