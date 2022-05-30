using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    [Header("Weapon Parameters")]
    public float damage;
    public string targetType;
    public float attackInterval;
    public float weaponRotationOffset = 0;
    public GameObject defaultDamageDealer;

    protected float timeToNextAttack;

    
}
