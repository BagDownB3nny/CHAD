using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyRangedWeapon : RangedWeapon
{
    public GameObject target;

    //returns a normalized direction vector from obj to end
    public void FiringDirection() {
        directionVector = (target.transform.position - transform.position).normalized;
        directionRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
    }

    public void SetTarget(GameObject _target) {
        target = _target;
    }
}
