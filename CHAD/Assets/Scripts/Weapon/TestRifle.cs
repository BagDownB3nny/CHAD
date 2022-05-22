using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRifle : PlayerRangedWeapon
{
    void Update()
    {
<<<<<<< HEAD

=======
>>>>>>> fb352e443d04cbd3d1c5459925eacfd2e39cf62d
        FiringDirection();
        PointAtMouse();
        if (Input.GetMouseButton(0)){
            Attack();
        }
    }

    public override void Attack() {
        if (timeToNextShot <= 0) {
            BulletDirection();
            GameObject shot = Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, directionRotation + projectileRotationOffset));
            shot.GetComponent<ProjectileStatsManager>().SetStats(holder, holderAttack, holderArmourPenetration, speed, 
                    damage, range, targetType, gameObject, transform.position, bulletDirectionVector, projectileRotationOffset);    
            
            timeToNextShot = shotInterval;
        } else {
            timeToNextShot -= Time.deltaTime;
        }
    }    
}
