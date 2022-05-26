using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : CharacterStatsManager
{
    [Header("Network ID")]
    public string enemyRefId;

    protected override void Awake() {
        base.Awake();
    }
}
