using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    protected override void Awake() {
        base.Awake();
        characterType = CharacterType.Player;
    }
}
