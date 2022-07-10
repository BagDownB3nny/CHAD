using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : BossRangedWeapon
{
    private new void Update()
    {
        base.Update();
        directionRotation += 5f;
    }
}
