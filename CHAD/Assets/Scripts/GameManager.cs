using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerWeapons {
    TestRifle = 1
}

public enum Enemies {
    MaskedGuy = 0,
    WhiteDude = 1
}

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public List<GameObject> playerPrefabs;
    public Dictionary<int, GameObject> players;
    public List<GameObject> enemyPrefabs;
    public List<GameObject> enemies;
    public Dictionary<int, GameObject> projectiles;
    private int placeholderInt = -1;

    public int projectileId {get; private set;}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            players = new Dictionary<int, GameObject>();
            enemies = new List<GameObject>();
            projectiles = new Dictionary<int, GameObject>();

            projectileId = 0;

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
        if (NetworkManager.gameType == GameType.Client)
        {
            if (receiving)
            {
                GameObject player = Instantiate(playerPrefabs[characterType]);
                player.GetComponent<PlayerStatsManager>().myId = id;
                players.Add(id ,player);
            } else
            {
                ClientSend.SpawnPlayer(characterType, position);
            }
        }
        if (NetworkManager.gameType == GameType.Server)
        {
            players.Add(id, Instantiate(playerPrefabs[characterType]));
        }
    }

#endregion

#region PlayerAttack
    public void PlayerAttack(int playerId, PlayerWeapons gunType, float directionRotation) {
        Debug.Log("Server GameManager receives attack");
        Debug.Log("Wpn manager:" + players[playerId].GetComponent<PlayerWeaponsManager>());
        Debug.Log("Wpn script: " + players[playerId].GetComponent<PlayerWeaponsManager>().weaponScript);
        object[] bulletInfo = players[playerId].GetComponent<PlayerWeaponsManager>().weaponScript
            .Attack(gunType, directionRotation);
        
        if (bulletInfo != null) {
            projectiles.Add(projectileId, (GameObject) bulletInfo[0]);
            ServerSend.PlayerAttack(playerId, projectileId, gunType, (float) bulletInfo[1]);
            projectileId++;
        }
    }

    public void ReceivePlayerAttack(int playerId, int projectileRefId, PlayerWeapons gunType, float bulletDirectionRotation) {
        Debug.Log("Client GameManager receives attack");
        GameObject bullet = players[playerId].GetComponent<PlayerWeaponsManager>().weaponScript
            .ReceiveAttack(gunType, bulletDirectionRotation);
        projectiles.Add(projectileRefId, bullet);
    }
#endregion

#region SpawnEnemy

    public void SpawnEnemy(GameObject _enemy, int _id, Vector2 _position) {
        enemies.Add(_enemy);
        ServerSend.SpawnEnemy(_id, _position);
    }

    public void ReceiveSpawnEnemy(int _enemyId, Vector2 _position) {
        //might want to shift the actual instantiation to anotehr script?
        Debug.Log("client spawning enemy");
        enemies.Add(Instantiate(enemyPrefabs[_enemyId], _position, Quaternion.identity));
    }

#endregion

}
