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
            if (CanAttack()) {
                Attack();
            }
        }
    }
}
