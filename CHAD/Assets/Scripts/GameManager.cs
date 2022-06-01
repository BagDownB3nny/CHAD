using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CharacterType {
    Player = 0,
    Enemy = 1
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
        SpawnPlayer(PlayerClient.instance.myId.ToString(), 1, new Vector2(0, 0));
    }

    public void SpawnPlayer(string id, int characterType, Vector2 position, bool receiving = false)
    {
        if (NetworkManager.gameType == GameType.Client)
        {
            if (receiving)
            {
                Debug.Log("Spawning player of type " + characterType);
                GameObject player = Instantiate(playerPrefabs[characterType]);
                player.GetComponent<PlayerStatsManager>().characterClass = characterType;
                player.GetComponent<PlayerStatsManager>().characterRefId = id;
                players.Add(id ,player);
            } else
            {
                ClientSend.SpawnPlayer(characterType, position);
            }
        }
        if (NetworkManager.gameType == GameType.Server)
        {
            GameObject player = Instantiate(playerPrefabs[characterType]);
            Debug.Log("GameManager Server: Spawning character of type " + characterType);
            player.GetComponent<PlayerStatsManager>().characterClass = characterType;
            player.GetComponent<PlayerStatsManager>().characterRefId = id;
            players.Add(id ,player);
        }
    }

#endregion
    public void Disconnect(int _playerRefId) {
        Destroy(GameManager.instance.players[_playerRefId.ToString()]);
        GameManager.instance.players.Remove(_playerRefId.ToString());
        ServerSend.DisconnectPlayer(_playerRefId.ToString());
    }
}
