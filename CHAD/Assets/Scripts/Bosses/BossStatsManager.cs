using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatsManager : CharacterStatsManager
{
    protected override void Awake()
    {
        base.Awake();
        characterType = CharacterType.Boss;
    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
    }
}
