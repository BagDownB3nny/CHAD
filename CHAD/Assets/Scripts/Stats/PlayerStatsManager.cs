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

    private void OnDestroy()
    {
        PlayerInfoManager.AllPlayerInfo[characterRefId].SetStats(this);
    }
}
