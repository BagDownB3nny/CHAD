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
    public BossWeaponType bossWeaponType;
    [SerializeField]
    public bool targetsPlayers;

    private void Awake()
    {
        holder = BossManager.instance.gameObject;
    }

    public void Update()
    {
        if (NetworkManager.gameType == GameType.Server)
        {
            if (timeToWeaponExpiry < 0)
            {
                if (bossWeaponType == BossWeaponType.primary)
                {
                    holder.GetComponent<BossAttacker>().EndAttack("primary");
                } else
                {
                    holder.GetComponent<BossAttacker>().EndAttack("secondary");
                }
                Destroy(gameObject);
            } else
            {
                timeToWeaponExpiry -= Time.deltaTime;
            }
            if (targetsPlayers)
            {
                GetComponent<BossAttacker>().FindTarget();
                CalculateDirectionVector();
            }
            // PointToTarget();
            //ServerSend.RotateRangedWeapon(holder.GetComponent<CharacterStatsManager>().characterType,
                    //holder.GetComponent<CharacterStatsManager>().characterRefId, directionRotation);
            if (CanAttack())
            {
                Attack();
            }
            else
            {
                timeToNextAttack -= Time.deltaTime;
            }
        }
    }
}
