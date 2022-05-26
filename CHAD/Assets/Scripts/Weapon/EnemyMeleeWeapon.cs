using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMeleeWeapon : MeleeWeapon
{
    void Update()
    {
        if (CanAttack()) {
            Attack();
        }
    }
}
