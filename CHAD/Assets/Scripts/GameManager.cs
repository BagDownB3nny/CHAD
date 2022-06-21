using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CharacterType {
    Player = 0,
    Enemy = 1
}

public enum PlayerClasses {
    Captain = 0,
    Medic = 1,
    Bastion = 2,
    Assassin = 3,
    Engineer = 4
}

public enum PlayerWeapons {
    TestRifle = 1,
    Crossbow = 2,
    Minigun = 3
}

public enum Enemies {
    MaskedGuy = 0,
    WhiteDude = 1
}

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public List<GameObject> playerPrefabs;
    public List<GameObject> enemyPrefabs;

    public Dictionary<string, GameObject> spawners;
    public Dictionary<string, GameObject> players;
    public Dictionary<string, PlayerClasses> playerClasses;
    public Dictionary<string, GameObject> enemies;
    public Dictionary<string, GameObject> projectiles;
    public Dictionary<string, GameObject> damageDealers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            spawners = new Dictionary<string, GameObject>();
            players = new Dictionary<string, GameObject>();
            playerClasses = new Dictionary<string, PlayerClasses>();
            projectiles = new Dictionary<string, GameObject>();
            enemies = new Dictionary<string, GameObject>();
            damageDealers = new Dictionary<string, GameObject>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //temporary manual adding of spawners into the spawners dictionary for testing
        spawners.Add("MGS0", GameObject.Find("MaskedGuySpawner"));
        spawners.Add("WDS0", GameObject.Find("WhiteDudeSpawner"));
    }

    
#region SpawnPlayer
    public void SpawnWaitingRoomPlayer()
    {
        SpawnPlayer(PlayerClient.instance.myId.ToString(), 2, new Vector2(0, 0));
    }

    public void SpawnPlayer(string _playerRefId, int _playerClass, Vector2 _position, bool _receiving = false)
    {
        if (NetworkManager.gameType == GameType.Client)
        {
            if (_receiving)
            {
                GameObject player = Instantiate(playerPrefabs[_playerClass], _position, Quaternion.identity);
                player.GetComponent<PlayerStatsManager>().playerClass = _playerClass;
                player.GetComponent<PlayerStatsManager>().characterRefId = _playerRefId;
                if (NetworkManager.IsMine(_playerRefId)) {
                    player.GetComponent<PlayerStatsManager>().InitializeHealthBar();
                }
                players.Add(_playerRefId ,player);
            } else
            {
                ClientSend.SpawnPlayer(_playerClass, _position);
            }
        }
        if (NetworkManager.gameType == GameType.Server)
        {
            GameObject player = Instantiate(playerPrefabs[_playerClass], _position, Quaternion.identity);
            player.GetComponent<PlayerStatsManager>().playerClass = _playerClass;
            player.GetComponent<PlayerStatsManager>().characterRefId = _playerRefId;
            players.Add(_playerRefId ,player);
        }
    }

#endregion
    public void RemovePlayer(int _playerRefId) {
        Destroy(GameManager.instance.players[_playerRefId.ToString()]);
        GameManager.instance.players.Remove(_playerRefId.ToString());
        ServerSend.RemovePlayer(_playerRefId.ToString());
    }

    public void ChangeClass(int _playerRefId, int _playerClass) {
        Vector2 playerPosition = GameManager.instance.players[_playerRefId.ToString()].transform.position;
        RemovePlayer(_playerRefId);
        SpawnPlayer(_playerRefId.ToString(), _playerClass, playerPosition);
        ServerSend.ChangeClass(_playerRefId, _playerClass, playerPosition);
    }

    public void SendChangeClass(PlayerClasses _playerClass) {
        ClientSend.ChangeClass((int) _playerClass);
    }

    public void ReceiveChangeClass(int _playerRefId, int _playerClass, Vector2 _playerPosition) {
        SpawnPlayer(_playerRefId.ToString(), _playerClass, _playerPosition, true);
    }

    public void ResetGame() {
        foreach (KeyValuePair<string, GameObject> pair in players) {
            Destroy(pair.Value);
        }
        players.Clear();
        foreach (KeyValuePair<string, GameObject> pair in projectiles) {
            Destroy(pair.Value);
        }
        projectiles.Clear();
        foreach (KeyValuePair<string, GameObject> pair in enemies) {
            Destroy(pair.Value);
        }
        enemies.Clear();
        foreach (KeyValuePair<string, GameObject> pair in damageDealers) {
            Destroy(pair.Value);
        }
        damageDealers.Clear();
    }

    public void Broadcast(string _msg) {
        Debug.Log(_msg);
    }
}
