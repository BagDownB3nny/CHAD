using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstShooter : WeaponShooter
{
    public float burstInterval;
    public int burstAmount;

    private int burstsFired = 0;
    private float timeToNextBurst = 0;

    private bool isBursting = false;

    private void Start()
    {
        RangedWeapon weapon = GetComponent<RangedWeapon>();
        if (weapon.attackInterval < burstInterval)
        {
            Debug.Log("WARNING: " + weapon.name + " has attackInterval smaller than burstInterval!");
        }
    }

    private void Update()
    {
        if (NetworkManager.gameType == GameType.Server && isBursting)
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        isBursting = true;
        if (timeToNextBurst <= 0)
        {
            RangedWeapon weapon = GetComponent<RangedWeapon>();
            CharacterStatsManager characterStats = weapon.holder.GetComponent<CharacterStatsManager>();

            GameObject shot = Instantiate(weapon.projectile, transform.position,
                            Quaternion.Euler(0f, 0f, weapon.bulletDirectionRotation + weapon.projectileRotationOffset));
            string projectileRefId = string.Format("{0}.{1}.{2}", characterStats.characterRefId,
                    characterStats.localProjectileRefId, burstsFired);
            shot.GetComponent<ProjectileStatsManager>().SetStats(projectileRefId, weapon.holder, weapon,
                    gameObject, weapon.bulletDirectionVector, weapon.projectileRotationOffset);
            GameManager.instance.projectiles.Add(projectileRefId, shot);
            ServerSend.RangedAttack(characterStats.characterType, characterStats.characterRefId,
                    projectileRefId, weapon.bulletDirectionRotation);

            // Incrementing burstsFired and timeToNextBurst
            burstsFired++;
            timeToNextBurst = burstInterval;

            // Reset weapon when all bursts are fired
            if (burstsFired == burstAmount)
            {
                characterStats.localProjectileRefId++;
                weapon.timeToNextAttack = weapon.attackInterval;
                timeToNextBurst = 0;
                burstsFired = 0;
                isBursting = false;
            }
        } else
        {
            timeToNextBurst -= Time.deltaTime;
        }
    }

    public override void ReceiveShoot(string _projectileRefId, float _projectileDirectionRotation)
    {
        RangedWeapon weapon = GetComponent<RangedWeapon>();

        GameObject shot = Instantiate(weapon.projectile, transform.position,
                Quaternion.Euler(0f, 0f, _projectileDirectionRotation + weapon.projectileRotationOffset));
        shot.GetComponent<ProjectileStatsManager>().SetStats(_projectileRefId,
                weapon.holder, weapon, gameObject, weapon.bulletDirectionVector, weapon.projectileRotationOffset);

        GameManager.instance.projectiles.Add(_projectileRefId, shot);
        weapon.timeToNextAttack = weapon.attackInterval;

        burstsFired++;

        // Reset weapon when all bursts are fired
        if (burstsFired == burstAmount)
        {
            weapon.timeToNextAttack = weapon.attackInterval;
            timeToNextBurst = 0;
            burstsFired = 0;
        }
    }
}
