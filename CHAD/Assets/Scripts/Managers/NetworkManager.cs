using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameType {
    Singleplayer,
    Client,
    Server
}

public static class NetworkManager
{

    public static GameType gameType;

    public static void SetGameType(GameType _gameType)
    {
        gameType = _gameType;
    }

    public static bool IsMine(string _id) {
        return NetworkManager.gameType == GameType.Client
                && string.Equals(PlayerClient.instance.myId.ToString(), _id); 
    }
}
