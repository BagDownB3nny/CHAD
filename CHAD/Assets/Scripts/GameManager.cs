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
    public List<GameObject> enemyPrefabs;

    public Dictionary<int, GameObject> players;
    public Dictionary<int, GameObject> enemies;
    public Dictionary<int, GameObject> projectiles;

    public int enemyRefId {get; private set;} = 0;

    public int projectileId {get; private set;} = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            players = new Dictionary<int, GameObject>();
            projectiles = new Dictionary<int, GameObject>();
            enemies = new Dictionary<int, GameObject>();
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
        object[] bulletInfo = players[playerId].GetComponent<PlayerWeaponsManager>().weaponScript
            .Attack(gunType, directionRotation);
        if (bulletInfo != null) {
            GameObject bullet = (GameObject)bulletInfo[0];
            bullet.GetComponent<ProjectileStatsManager>().id = projectileId;
            Debug.Log("Projectile id: " + projectileId);
            projectiles.Add(projectileId, bullet);
            ServerSend.PlayerAttack(playerId, projectileId, gunType, (float) bulletInfo[1]);
            projectileId++;
        }
    }

    public void ReceivePlayerAttack(int playerId, int projectileRefId, PlayerWeapons gunType, float bulletDirectionRotation) {
        GameObject bullet = players[playerId].GetComponent<PlayerWeaponsManager>().weaponScript
            .ReceiveAttack(gunType, bulletDirectionRotation);
        Debug.Log("Projectile ref id: " + projectileRefId);
        projectiles.Add(projectileRefId, bullet);
    }
#endregion

#region SpawnEnemy

    public void SpawnEnemy(GameObject _enemy, int _id, Vector2 _position) {
        enemies.Add(enemyRefId, _enemy);
        _enemy.GetComponent<EnemyStatsManager>().enemyRefId = enemyRefId;
        ServerSend.SpawnEnemy(enemyRefId, _id, _position);
        enemyRefId++;
    }

    public void ReceiveSpawnEnemy(int _enemyRefId, int _enemyId, Vector2 _position) {
        //might want to shift the actual instantiation to anotehr script?
        Debug.Log("client spawning enemy");
        GameObject enemySpawned = Instantiate(enemyPrefabs[_enemyId], _position, Quaternion.identity);
        enemySpawned.GetComponent<EnemyStatsManager>().enemyRefId = _enemyRefId;
        enemies.Add(_enemyRefId, enemySpawned);
    }

#endregion

#region MoveEnemy

    public void MoveEnemy(int _enemyRefId, Vector2 _position) {
        enemies[_enemyRefId].transform.position = _position;
    }

#endregion

}
