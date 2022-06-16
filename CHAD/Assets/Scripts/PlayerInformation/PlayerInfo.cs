using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerInfo
{
    public string characterRefId;
    public PlayerClasses playerClass;
    public float maxHp;
    public float hp;
    public float attack;
    public float speed;
    public float armour;
    public float armourPenetration;
    public float armourEffectiveness;
    public float proficiency;
    public Dictionary<int, GameObject> weaponInventory = new Dictionary<int, GameObject>(8);

    public void UpdateStats(string stat, float newValue) {
        FieldInfo field = typeof(PlayerInfo).GetField(stat);
        field.SetValue(this, newValue);
    }

    public void ChangeClass(PlayerClasses _playerClass, PlayerStatsManager stats)
    {
        playerClass = _playerClass;
        SetStats(stats);
    }

    // Constructor that takes in characterRefId, playerClass and the stats of the player GameObject
    public PlayerInfo(string _characterRefId, PlayerClasses _playerClass, PlayerStatsManager stats) {
        characterRefId = _characterRefId;
        playerClass = _playerClass;
        SetStats(stats);
    }

    public void SetStats(PlayerStatsManager stats) {
        maxHp = stats.maxHp;
        hp = stats.hp;
        attack = stats.attack;
        speed = stats.speed;
        armour = stats.armour;
        armourPenetration = stats.armourPenetration;
        armourEffectiveness = stats.armourEffectiveness;
        proficiency = stats.proficiency;
    }
}
