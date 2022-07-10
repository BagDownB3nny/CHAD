using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossWeaponType
{
    primary = 0,
    secondary = 1
}

public class BossRangedWeapon : RangedWeapon
{
    [SerializeField]
    private float timeToWeaponExpiry;
    [SerializeField]
    private BossWeaponType bossWeaponType;

    void Update()
    {
        if (NetworkManager.gameType == GameType.Server)
        {
            if (timeToWeaponExpiry < 0)
            {
                if (bossWeaponType == BossWeaponType.primary)
                {
                    holder.GetComponent<BossAttacker>().isPrimaryAttacking = false;
                } else
                {
                    holder.GetComponent<BossAttacker>().isSecondaryAttacking = false;
                }
                Destroy(gameObject);
            } else
            {
                timeToWeaponExpiry -= Time.deltaTime;
            }
            // TODO: Set directionvector and directionrotation
            PointToTarget();
            ServerSend.RotateRangedWeapon(holder.GetComponent<CharacterStatsManager>().characterType,
                    holder.GetComponent<CharacterStatsManager>().characterRefId, directionRotation);
            if (CanAttack())
            {
                holder.GetComponent<BossStatsManager>().characterRefId = "primary";
                Attack();
            }
            else
            {
                timeToNextAttack -= Time.deltaTime;
            }
        }
    }
}
