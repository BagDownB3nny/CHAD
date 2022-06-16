using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //these will be manually set by the weapons manager when it instantiates this weapon
    [Header("Holder Parameters")]
    public GameObject holder;
    public float damage;
    public string targetType;
    public float attackInterval;
    public float weaponRotationOffset;

    [Header("Calculation parameters")]
    public float timeToNextAttack = 0;


    public void Unequip() {
        Destroy(gameObject);
    }

    public abstract void Attack();

    public bool CanAttack() {
        if (timeToNextAttack <= 0) {
            return true;
        } else {
            return false;
        }
    }
}
