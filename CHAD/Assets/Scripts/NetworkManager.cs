using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameType {
    Singleplayer,
    Client,
    Server
}

public class NetworkManager : MonoBehaviour
{

    public GameType gameType;

    public static NetworkManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetGameType(GameType _gameType)
    {
        gameType = _gameType;
    }
}
