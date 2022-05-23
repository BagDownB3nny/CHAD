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

    public static bool IsMine(int id)
    {
        if (NetworkManager.gameType == GameType.Client && PlayerClient.instance.myId == id)
        {
            return true;
        }
        return false;
    }
}
