using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerRangedWeapon : RangedWeapon
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

    //these will be manually set by the weapons manager when it instantiates this weapon
    [Header("Holder Parameters")]
    public GameObject holder;
    public float holderAttack;
    public float holderArmourPenetration;

    //points this gameObject at the mouse
    public void PointAtMouse() {
        transform.rotation = Quaternion.Euler(0f, 0f, directionRotation + gunRotationOffset);
        if (directionRotation >= -90 && directionRotation < 89) {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        } else {
            gameObject.transform.localScale = new Vector3(1, -1, 1);
        }
    }

    //returns a normalized direction vector from obj to end
    public void FiringDirection() {
        directionVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
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

    public void Discard() {
        Destroy(gameObject);
    }
}
