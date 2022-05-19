using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyRangedWeapon : RangedWeapon
{
    [Header("Weapon Parameters")]
    public GameObject target;
    public float speed;
    public float damage;
    public float range;
    public string targetType;
    public float accuracy;
    public float shotInterval;
    public float gunRotationOffset = 0;
    public float projectileRotationOffset;
    public GameObject projectile;

    [Header("Projectile Shooting Parameters")]
    public Vector3 directionVector;
    public Vector3 bulletDirectionVector;
    public float directionRotation;
    public float timeToNextShot;

    //these will be set by the weapons manager when it instantiates this weapon
    [Header("Holder Parameters")]
    public GameObject holder;
    public float holderAttack;
    public float holderArmourPenetration;

    //returns a normalized direction vector from obj to end
    public void FiringDirection() {
        directionVector = (target.transform.position - transform.position).normalized;
        directionRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
    }

    public void BulletDirection() {
        float _accuracy = 10 - accuracy;
        float rand = Random.Range(-_accuracy, _accuracy);
        bulletDirectionVector = Quaternion.AngleAxis(rand, Vector3.back) * directionVector;
    }

    public void SetAttackStats(GameObject _holder, float _attack, float armourPenetration) {
        holder = _holder;
        holderAttack = _attack;
        holderArmourPenetration = armourPenetration;
    }

    public void SetTarget(GameObject _target) {
        target = _target;
    }

    public void Discard() {
        Destroy(gameObject);
    }
}
