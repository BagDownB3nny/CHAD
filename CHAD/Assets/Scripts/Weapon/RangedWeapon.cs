using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    [Header("Weapon Parameters")]
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

    public void BulletDirection() {
        float _accuracy = 10 - accuracy;
        float rand = Random.Range(-_accuracy, _accuracy);
        bulletDirectionVector = Quaternion.AngleAxis(rand, Vector3.back) * directionVector;
    }
}
