using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyRangedWeapon : RangedWeapon
{
    void Update()
    {
        if (DistanceToTarget() > 30)
        {
            return;
        }

        if (NetworkManager.gameType == GameType.Server) {

            CalculateDirectionVector();
            
            PointToTarget();
            ServerSend.RotateRangedWeapon(holder.GetComponent<CharacterStatsManager>().characterType, 
                    holder.GetComponent<CharacterStatsManager>().characterRefId, directionRotation);
            if (CanAttack()) {
                Attack();
            } else {
                timeToNextAttack -= Time.deltaTime;
            }
        }
    }

    private float DistanceToTarget()
    {
        GameObject target = holder.GetComponent<CharacterStatsManager>().target;
        if (target != null)
        {
            return Vector3.Distance(transform.position, target.transform.position);
        }
        else
        {
            return float.MaxValue;
        }
    }
}
