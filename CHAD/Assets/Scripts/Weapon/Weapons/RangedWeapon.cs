using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    [Header("Weapon Parameters")]
    public float speed;
    public float range;
    public float accuracy;
    public float projectileRotationOffset;
    public GameObject projectile;

    [Header("Projectile Shooting Parameters")]
    public Vector2 directionVector;
    public float directionRotation;
    public Vector3 bulletDirectionVector;
    public float bulletDirectionRotation;

    public void CalculateDirectionVector() {
        GameObject target = holder.GetComponent<CharacterStatsManager>().target;
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            directionVector = (targetPosition - transform.position).normalized;
            directionRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        } else
        {
            holder.GetComponent<EnemyStatsManager>().FindTarget();
        }
    }

    public override void Attack() {
        if (CanAttack()) {
            CharacterStatsManager characterStats = holder.GetComponent<CharacterStatsManager>();
            CalculateBulletDirection();
            GetComponent<WeaponShooter>().Shoot();
        }
    }

    public void ReceiveAttack(string _projectileRefId, float _projectileDirectionRotation) {
        if (NetworkManager.IsMine(holder.GetComponent<CharacterStatsManager>().characterRefId))
        {
            SoundManager.instance.PlaySound(Sounds.TestRifleShot);
        }
        bulletDirectionVector = Quaternion.AngleAxis(_projectileDirectionRotation, Vector3.forward) * Vector2.right;
        GetComponent<WeaponShooter>().ReceiveShoot(_projectileRefId, _projectileDirectionRotation);
    }

    public void PointToTarget() {
        transform.rotation = Quaternion.Euler(0f, 0f, directionRotation + weaponRotationOffset);
        if (directionRotation >= -90 && directionRotation < 89)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1, -1, 1);
        }
    }

    public void ReceiveRotateRangedWeapon(float _directionRotation) {
        directionRotation = _directionRotation;
        directionVector = Quaternion.Euler(0, 0, directionRotation) * Vector2.right;
        transform.rotation = Quaternion.Euler(0, 0, _directionRotation);
    }

    public void CalculateBulletDirection() {
        float _accuracy = 10 - accuracy;
        float rand = Random.Range(-_accuracy, _accuracy);
        bulletDirectionVector = (Quaternion.AngleAxis(rand, Vector3.back) * directionVector).normalized;
        bulletDirectionRotation = Mathf.Atan2(bulletDirectionVector.y, bulletDirectionVector.x) * Mathf.Rad2Deg;
    }
}
