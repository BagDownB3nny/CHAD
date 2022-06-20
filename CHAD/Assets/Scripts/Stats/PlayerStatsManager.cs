using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    public int playerClass;
    protected override void Awake() {
        base.Awake();
        characterType = CharacterType.Player;
    }

    public void InitializeHealthBar() {
        healthBar = GameUIManager.instance.healthBar.GetComponent<HealthBar>();
        healthBar.Initialize(hp);
    }
}
