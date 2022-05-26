using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyRangedWeapon : RangedWeapon
{
    void Update()
    {
        CalculateDirectionVector();
        PointToTarget();
        if (CanAttack()) {
            Attack();
        }
    }
}
