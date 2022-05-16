using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyProjectile : Projectile
{
    public abstract void SetOrigin(GameObject origin);
    public abstract void UpdateOriginTargetStatus(bool targetStatus);
}
