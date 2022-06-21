using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyRangedWeapon : RangedWeapon
{
    void Update()
    {
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
}
