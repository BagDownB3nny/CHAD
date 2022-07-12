using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{

    protected void Awake()
    {
        SetMovement();
        SetAttack();
    }

    public abstract void SetMovement();

    public abstract void SetAttack();
}
