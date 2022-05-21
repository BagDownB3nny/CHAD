using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //these will be manually set by the weapons manager when it instantiates this weapon
    [Header("Holder Parameters")]
    public GameObject holder;
    public float holderAttack;
    public float holderArmourPenetration;

    public abstract void Attack();

    public void SetAttackStats(GameObject _holder, float _attack, float armourPenetration) {
        holder = _holder;
        holderAttack = _attack;
        holderArmourPenetration = armourPenetration;
    }

    public void Discard() {
        Destroy(gameObject);
    }
}
