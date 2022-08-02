using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooter : WeaponShooter
{
    public override void Shoot()
    {
        RangedWeapon weapon = GetComponent<RangedWeapon>();
        CharacterStatsManager characterStats = weapon.holder.GetComponent<CharacterStatsManager>();

        GameObject shot = Instantiate(weapon.projectile, transform.position,
                     Quaternion.Euler(0f, 0f, weapon.bulletDirectionRotation + weapon.projectileRotationOffset));
        string projectileRefId = string.Format("{0}.{1}", characterStats.characterRefId,
                characterStats.localProjectileRefId);
        characterStats.localProjectileRefId++;
        shot.GetComponent<ProjectileStatsManager>().SetStats(projectileRefId, weapon.holder, weapon,
                gameObject, weapon.bulletDirectionVector, weapon.projectileRotationOffset);
        GameManager.instance.projectiles.Add(projectileRefId, shot);
        ServerSend.RangedAttack(characterStats.characterType, characterStats.characterRefId,
                projectileRefId, weapon.bulletDirectionRotation);
        weapon.timeToNextAttack = weapon.attackInterval;
    }

    public override void ReceiveShoot(string _projectileRefId, float _projectileDirectionRotation)
    {
        RangedWeapon weapon = GetComponent<RangedWeapon>();

        GameObject shot = Instantiate(weapon.projectile, transform.position,
                Quaternion.Euler(0f, 0f, _projectileDirectionRotation + weapon.projectileRotationOffset));
        shot.GetComponent<ProjectileStatsManager>().SetStats(_projectileRefId,
                weapon.holder, weapon, gameObject, weapon.bulletDirectionVector, weapon.projectileRotationOffset); ;
        GameManager.instance.projectiles.Add(_projectileRefId, shot);
        weapon.timeToNextAttack = weapon.attackInterval;
    }
}
