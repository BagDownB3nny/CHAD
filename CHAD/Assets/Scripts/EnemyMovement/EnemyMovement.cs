using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    protected abstract void Move();

    protected abstract void Face();

    protected abstract void FindTarget();

    public abstract void SetStats(float _speed);

    //might not be needed since melee enemymovement doesnt need target information for weapon
    public abstract void UpdateWeaponTarget();
}
