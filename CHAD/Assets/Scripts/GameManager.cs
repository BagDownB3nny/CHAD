using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public List<GameObject> playerPrefabs;

    public Dictionary<int, GameObject> players;
    public Dictionary<int, GameObject> enemies;
    public Dictionary<int, GameObject> projectiles;
    private int placeholderInt = -1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            players = new Dictionary<int, GameObject>();
            enemies = new Dictionary<int, GameObject>();
            projectiles = new Dictionary<int, GameObject>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    
#region SpawnPlayer
    public void SpawnWaitingRoomPlayer()
    {
        SpawnPlayer(PlayerClient.instance.myId, 0, new Vector2(0, 0));
    }

    public void SpawnPlayer(int id, int characterType, Vector2 position, bool receiving = false)
    {
        if (NetworkManager.instance.gameType == GameType.Client)
        {
            if (receiving)
            {
                players.Add(id ,Instantiate(playerPrefabs[characterType]));
            } else
            {
                ClientSend.SpawnPlayer(characterType, position);
            }
        }
        if (NetworkManager.instance.gameType == GameType.Server)
        {
            players.Add(id, Instantiate(playerPrefabs[characterType]));
        }
    }
#endregion

#region Player Movement
    public void MovePlayer(bool[] _inputs) {
        MovePlayer(placeholderInt, _inputs);
    }
    
    public void MovePlayer(int id, bool[] _inputs, bool receiving = false)
    {
        if (NetworkManager.instance.gameType == GameType.Client)
        {
            if (receiving)
            {
                //TODO: move the player
            } else
            {
                //TODO: client send the movement
                //ClientSend.SpawnPlayer(characterType, position);
            }
        }
        if (NetworkManager.instance.gameType == GameType.Server)
        {
            //TODO: move the player on server side
            
        }
    }
#endregion

}
