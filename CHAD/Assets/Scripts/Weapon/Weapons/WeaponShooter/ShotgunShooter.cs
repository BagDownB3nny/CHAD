using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShooter : WeaponShooter
{
    public int bulletsPerShot;

    public override void Shoot()
    {
        RangedWeapon weapon = GetComponent<RangedWeapon>();
        CharacterStatsManager characterStats = weapon.holder.GetComponent<CharacterStatsManager>();
        string projectileRefId = string.Format("{0}.{1}", characterStats.characterRefId,
                    characterStats.localProjectileRefId);
        characterStats.localProjectileRefId++;
        float spreadAngle = 10 - weapon.accuracy;
        float angleBetweenBullets = spreadAngle / bulletsPerShot;
        for (int i = 0; i < bulletsPerShot; i++)
        {
            float bulletDirectionRotation = weapon.directionRotation - spreadAngle / 2
                    + i * angleBetweenBullets + weapon.projectileRotationOffset;
            Vector3 bulletDirectionVector = new Vector3( -Mathf.Sin(Mathf.Deg2Rad * bulletDirectionRotation),
                    Mathf.Cos(Mathf.Deg2Rad * bulletDirectionRotation), 0);
            GameObject shot = Instantiate(weapon.projectile, transform.position,
                         Quaternion.Euler(0f, 0f, bulletDirectionRotation));
            string shotgunShotRefId = projectileRefId + i.ToString();
            shot.GetComponent<ProjectileStatsManager>().SetStats(shotgunShotRefId, weapon.holder, weapon,
                    gameObject, bulletDirectionVector, weapon.projectileRotationOffset);
            GameManager.instance.projectiles.Add(shotgunShotRefId, shot);
        }
        ServerSend.RangedAttack(characterStats.characterType, characterStats.characterRefId,
                    projectileRefId, weapon.directionRotation);
        weapon.timeToNextAttack = weapon.attackInterval;
    }

    public override void ReceiveShoot(string _projectileRefId, float _projectileDirectionRotation)
    {
        Debug.Log("Received shotgun shot");
        RangedWeapon weapon = GetComponent<RangedWeapon>();
        float spreadAngle = 10 - weapon.accuracy;
        float angleBetweenBullets = spreadAngle / bulletsPerShot;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            float bulletDirectionRotation = _projectileDirectionRotation - spreadAngle / 2
                    + i * angleBetweenBullets + weapon.projectileRotationOffset;
            Vector3 bulletDirectionVector = new Vector3(Mathf.Sin(Mathf.Deg2Rad * bulletDirectionRotation),
                    Mathf.Cos(Mathf.Deg2Rad * bulletDirectionRotation), 0);
            GameObject shot = Instantiate(weapon.projectile, transform.position,
                         Quaternion.Euler(0f, 0f, bulletDirectionRotation));
            string shotgunShotRefId = _projectileRefId + i.ToString();
            shot.GetComponent<ProjectileStatsManager>().SetStats(shotgunShotRefId,
                    weapon.holder, weapon, gameObject, bulletDirectionVector, weapon.projectileRotationOffset); ;
            GameManager.instance.projectiles.Add(shotgunShotRefId, shot);
        }
        weapon.timeToNextAttack = weapon.attackInterval;
    }
}
