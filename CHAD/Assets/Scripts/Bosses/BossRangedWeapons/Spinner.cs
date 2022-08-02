using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : BossRangedWeapon
{
    [SerializeField]
    float spinSpeed;

    private new void Update()
    {
        base.Update();
        directionRotation += spinSpeed;
    }
}
