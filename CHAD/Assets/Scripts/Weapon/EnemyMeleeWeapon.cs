using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMeleeWeapon : MeleeWeapon
{
    [Header("Weapon Parameters")]
    public float damage;
    public float range;
    public string targetType;
    public float shotInterval;
    public float gunRotationOffset = 0;

    private float timeToNextShot;

    public void Discard() {
        Destroy(gameObject);
    }
}
