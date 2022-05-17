using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyProjectile : Projectile
{
    public abstract void SetOrigin(GameObject origin);
    //updates the origin on targetStatus
    public abstract void UpdateOriginTargetStatus(bool targetStatus);
}
