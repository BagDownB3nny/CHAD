using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    [Header("Network Id")]
    public string playerRefId;

    protected override void Awake() {
        base.Awake();
    }
}
