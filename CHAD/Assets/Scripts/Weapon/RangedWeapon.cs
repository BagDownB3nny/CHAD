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
        Vector3 targetPosition = holder.GetComponent<CharacterStatsManager>().target.transform.position;
        directionVector = (targetPosition - transform.position).normalized;
        directionRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
    }

    public override void Attack() {
        CharacterStatsManager characterStats = holder.GetComponent<CharacterStatsManager>();
        CalculateBulletDirection();
        GameObject shot = Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, bulletDirectionRotation + projectileRotationOffset));
        string projectileRefId = string.Format("{0}.{1}", characterStats.characterRefId, 
                characterStats.localProjectileRefId);
        characterStats.localProjectileRefId++;
        shot.GetComponent<ProjectileStatsManager>().SetStats(projectileRefId, holder, this, gameObject, bulletDirectionVector, projectileRotationOffset);    
        timeToNextAttack = attackInterval;
        GameManager.instance.projectiles.Add(projectileRefId, shot);
        ServerSend.RangedAttack(characterStats.characterType, characterStats.characterRefId, 
                projectileRefId, bulletDirectionRotation);
    }

    public void ReceiveAttack(string _projectileRefId, float _bulletDirectionRotation) {
        bulletDirectionVector = Quaternion.AngleAxis(_bulletDirectionRotation, Vector3.forward) * Vector2.right;
        GameObject shot = Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, _bulletDirectionRotation + projectileRotationOffset));
        shot.GetComponent<ProjectileStatsManager>().SetStats(_projectileRefId, 
                holder, this, gameObject, bulletDirectionVector, projectileRotationOffset);
        GameManager.instance.projectiles.Add(_projectileRefId, shot);
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
        transform.rotation = Quaternion.Euler(0, 0, _directionRotation);
    }

    public void CalculateBulletDirection() {
        float _accuracy = 10 - accuracy;
        float rand = Random.Range(-_accuracy, _accuracy);
        bulletDirectionVector = (Quaternion.AngleAxis(rand, Vector3.back) * directionVector).normalized;
        bulletDirectionRotation = Mathf.Atan2(bulletDirectionVector.y, bulletDirectionVector.x) * Mathf.Rad2Deg;
    }
}
