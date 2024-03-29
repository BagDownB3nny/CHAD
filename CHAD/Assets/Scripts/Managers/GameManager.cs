using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum PlayerItems
{
    None = 0,
    Boot = 1,
    Armour = 2,
    BFSword = 3,
    ElvenAccuracy = 4,
    PiercingRounds = 5
}

public enum CharacterType {
    Player = 0,
    Enemy = 1,
    Boss = 2
}

public enum PlayerClasses {
    Captain = 0,
    Medic = 1,
    Bastion = 2,
    Assassin = 3,
    Engineer = 4
}

public enum PlayerWeapons {
    None = 0,
    TestRifle = 1,
    Crossbow = 2,
    ToxicGun = 3
}

public enum Enemies {
    MaskedGuy = 0,
    WhiteDude = 1
}

public enum Bosses {
    Forest = 0,
    City = 1,
    Desert = 2,
}

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager instance;

    // List of prefabs
    public List<GameObject> playerPrefabs;
    public List<GameObject> enemyPrefabs;
    public List<GameObject> bossPrefabs;
    public List<GameObject> gunPrefabs;
    public List<GameObject> itemDrops;

    public Dictionary<string, GameObject> enemySpawners;
    // Dictionary of in-game objects
    public Dictionary<string, GameObject> spawners;
    public Dictionary<string, GameObject> players;
    public Dictionary<string, PlayerClasses> playerClasses;
    public Dictionary<string, GameObject> enemies;
    public Dictionary<string, GameObject> projectiles;
    public Dictionary<string, GameObject> damageDealers;

    // Level tracking data
    public int currentLevel = 0;
    private HashSet<int> bossLevels = new HashSet<int>() {2, 4, 6, 8, 10, 12};

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            enemySpawners = new Dictionary<string, GameObject>();
            players = new Dictionary<string, GameObject>();
            playerClasses = new Dictionary<string, PlayerClasses>();
            projectiles = new Dictionary<string, GameObject>();
            enemies = new Dictionary<string, GameObject>();
            damageDealers = new Dictionary<string, GameObject>();       }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //temporary manual adding of spawners into the spawners dictionary for testing
        // spawners.Add("MGS0", GameObject.Find("MaskedGuySpawner"));
        // spawners.Add("WDS0", GameObject.Find("WhiteDudeSpawner"));
    }

    public void ResetToMainMenu()
    {
        Destroy(MapManager.instance.gameObject);
        Destroy(GameUIManager.instance.gameObject);
        Destroy(CameraManager.instance.gameObject);
        SceneManager.LoadScene("StartMenu");
        Destroy(GameManager.instance.gameObject);
    }

    private void OnDestroy()
    {
        ResetGame();
        instance = null;
    }

    public void RemovePlayer(int _playerRefId) {
        Destroy(GameManager.instance.players[_playerRefId.ToString()]);
        GameManager.instance.players.Remove(_playerRefId.ToString());
        if (players.Count == 0)
        {
            ResetToMainMenu();
        }
        else
        {
            ServerSend.RemovePlayer(_playerRefId.ToString());
        }
    }

    //public void ChangeClass(int _playerRefId, int _playerClass) {
    //    Vector2 playerPosition = GameManager.instance.players[_playerRefId.ToString()].transform.position;
    //    RemovePlayer(_playerRefId);
    //    //SpawnPlayer(_playerRefId.ToString(), _playerClass, playerPosition);
    //    ServerSend.ChangeClass(_playerRefId, _playerClass, playerPosition);
    //}

    public void SendChangeClass(PlayerClasses _playerClass) {
        ClientSend.ChangeClass((int) _playerClass);
    }

    public void ReceiveChangeClass(int _playerRefId, int _playerClass, Vector2 _playerPosition) {
        //SpawnPlayer(_playerRefId.ToString(), _playerClass, _playerPosition, true);
    }

    public void NextGame() {
        int maxLevels = 5;
        if (currentLevel + 1 < maxLevels) {
            currentLevel++;
        }
        foreach (ServerClient serverClient in Server.serverClients.Values)
        {
            serverClient.spawnedIn = false;
        }
        ItemManager.instance.ResetItems();
        ResetGame();
    }

    public bool IsBossLevel()
    {
        return bossLevels.Contains(currentLevel);
    }

    public void ResetGame() {
        if (players.Count != 0)
        {
            foreach (KeyValuePair<string, GameObject> pair in players)
            {
                Destroy(pair.Value);
            }
            players.Clear();
        }
        if (projectiles.Count != 0)
        {
            foreach (KeyValuePair<string, GameObject> pair in projectiles)
            {
                Destroy(pair.Value);
            }
            projectiles.Clear();
        }
        if (enemies.Count != 0)
        {
            foreach (KeyValuePair<string, GameObject> pair in enemies)
            {
                Destroy(pair.Value);
            }
            enemies.Clear();
        }
        if (damageDealers.Count != 0)
        {
            foreach (KeyValuePair<string, GameObject> pair in damageDealers)
            {
                Destroy(pair.Value);
            }
            damageDealers.Clear();
        }
        if (enemySpawners.Count != 0)
        {
            foreach (KeyValuePair<string, GameObject> pair in enemySpawners)
            {
                Destroy(pair.Value);
            }
            enemySpawners.Clear();
        }
    }

    public void Broadcast(string _msg) {
        Debug.Log(_msg);
    }
}
