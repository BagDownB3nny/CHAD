using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void UpdateTargetStatus(bool targetStatus);

    //returns a random player if possible
    protected abstract GameObject FindTarget();

    //returns the current target
    public abstract GameObject GetTarget();
}
