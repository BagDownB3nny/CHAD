using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDemon : BossAttack
{
    public LineMover mover;
    BossRangedWeapon weapon;
    [SerializeField]
    float length;

    protected new void Awake()
    {
        mover = GetComponent<LineMover>();
        base.Awake();
    }

    public override void SetAttack()
    {
        return;
    }

    public override void SetMovement()
    {
        mover.SetLine(transform.position, transform.position + new Vector3(length, 0, 0));
    }
}
