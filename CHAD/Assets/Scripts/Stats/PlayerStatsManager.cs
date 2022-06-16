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

    public void SetStats(PlayerInfo playerInfo)
    {
        maxHp = playerInfo.maxHp;
        hp = playerInfo.hp;
        attack = playerInfo.attack;
        speed = playerInfo.speed;
        armour = playerInfo.armour;
        armourPenetration = playerInfo.armourPenetration;
        armourEffectiveness = playerInfo.armourEffectiveness;
        proficiency = playerInfo.proficiency;
    }

    public void InitializeHealthBar() {
        healthBar = GameUIManager.instance.healthBar.GetComponent<HealthBar>();
        healthBar.Initialize(hp);
    }
}
