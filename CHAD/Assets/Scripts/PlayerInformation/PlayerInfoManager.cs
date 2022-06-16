using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager {
    public static Dictionary<string, PlayerInfo> AllPlayerInfo = new Dictionary<string, PlayerInfo>();

    public static void Initialize(string playerRefId, PlayerClasses playerClass, PlayerStatsManager stats) {
        GameManager.instance.Broadcast("Player " + playerRefId + "is class" + playerClass.ToString());
        PlayerInfo playerInfo = new PlayerInfo(playerRefId, playerClass, stats);
        AllPlayerInfo.Add(playerRefId, playerInfo);
        GameManager.instance.Broadcast(playerRefId);
    }
}
