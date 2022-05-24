using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRifle : PlayerRangedWeapon
{

    public PlayerWeapons gunType = PlayerWeapons.TestRifle;
    void FixedUpdate()
    {
        if (NetworkManager.IsMine(holder.GetComponent<PlayerStatsManager>().playerId))
        {
            FiringDirection();
            PointAtMouse();
            if (Input.GetMouseButton(0)) {
                SendAttack();
            }
        }
    }

    public override object[] Attack(PlayerWeapons _gunType, float _directionRotation) {
        directionRotation = _directionRotation;
        directionVector = Quaternion.AngleAxis(-_directionRotation, Vector3.back) * Vector2.right;
        if (_gunType != gunType) {
            //Switch weapon
        }
        if (timeToNextShot <= 0) {
            BulletDirection();
            GameObject shot = Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, _directionRotation + projectileRotationOffset));
            shot.GetComponent<ProjectileStatsManager>().SetStats(holder, this, gameObject, bulletDirectionVector, projectileRotationOffset);    
            
            timeToNextShot = shotInterval;
            return new object[] {shot, bulletDirectionRotation};
        } else {
            timeToNextShot -= Time.deltaTime;
            return null;
        }
    }    

    public override void SendAttack() {
        ClientSend.SendAttack(gunType, directionRotation);
    }

    public override GameObject ReceiveAttack(PlayerWeapons gunType, float _bulletDirectionRotation) {
        bulletDirectionVector = Quaternion.AngleAxis(_bulletDirectionRotation, Vector3.forward) * Vector2.right;
        GameObject shot = Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, _bulletDirectionRotation + projectileRotationOffset));
        shot.GetComponent<ProjectileStatsManager>().SetStats(holder, this, gameObject, bulletDirectionVector, projectileRotationOffset);   
        return shot;
    }
}
