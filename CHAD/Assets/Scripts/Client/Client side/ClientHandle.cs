using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _msg = _packet.ReadString();

        Debug.Log($"Message from server: {_msg}");
        PlayerClient.instance.myId = _id;

        PlayerClient.instance.udp.Connect(
            ((IPEndPoint)PlayerClient.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        Debug.Log("CLIENT RECEIVED SPAWN DATA");
        int playerIdReceived = _packet.ReadInt();
        Vector2 position = _packet.ReadVector2();
        int characterType = _packet.ReadInt();
        GameManager.instance.SpawnPlayer(playerIdReceived.ToString(), characterType, position, true);
    }

    public static void MovePlayer(Packet _packet)
    {
        int _affectedPlayerId = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();
        GameManager.instance.players[_affectedPlayerId.ToString()].GetComponent<PlayerMovement>().ReceiveMovement(_position);
    }

    public static void PlayerAttack(Packet _packet) {
        string affectedPlayerId = _packet.ReadInt().ToString();
        int projectileRefId = _packet.ReadInt();
        PlayerWeapons gunType = (PlayerWeapons) _packet.ReadInt();
        float bulletDirectionRotation = _packet.ReadFloat();
        GameObject projectile = GameManager.instance.players[affectedPlayerId].GetComponent<PlayerWeaponsManager>().weaponScript
                .ReceiveAttack(gunType, bulletDirectionRotation);
        GameManager.instance.projectiles.Add(projectileRefId, projectile);
    }

    public static void SpawnEnemy(Packet _packet) {
        string spawnerRefId = _packet.ReadString();
        string enemyRefId = _packet.ReadString();
        int enemyId = _packet.ReadInt();
        Vector2 position = _packet.ReadVector2();
        Debug.Log(spawnerRefId);
        if (GameManager.instance.spawners.ContainsKey(spawnerRefId)) {
            Debug.Log("spawning enemy on client");
            GameManager.instance.spawners[spawnerRefId].GetComponent<EnemySpawner>().ReceiveSpawnEnemy(enemyRefId, enemyId, position);
        }
    }

    public static void ProjectileMovement(Packet _packet) {
        int _projectileRefId = _packet.ReadInt();
        Vector2 _position  = _packet.ReadVector2();
        if (GameManager.instance.projectiles.ContainsKey(_projectileRefId) && GameManager.instance.projectiles[_projectileRefId]) {
            Debug.Log("Moving Projectile:" + _projectileRefId);
            GameManager.instance.projectiles[_projectileRefId].GetComponent<ProjectileMovement>()
            .ReceiveMovement(_position);
        }
    }

    public static void DestroyProjectile(Packet _packet) {
        int _projectileRefId = _packet.ReadInt();
        if (GameManager.instance.projectiles.ContainsKey(_projectileRefId) && GameManager.instance.projectiles[_projectileRefId]) {
            Debug.Log("Destroying Projectile:" + _projectileRefId);
            GameManager.instance.projectiles[_projectileRefId].GetComponent<ProjectileMovement>()
                    .ReceiveDestroyProjectile();
        }
    }
    
    public static void MoveEnemy(Packet _packet) {
        string enemyRefId = _packet.ReadString();
        Vector2 position = _packet.ReadVector2();
        if (GameManager.instance.enemies.ContainsKey(enemyRefId)) {
            GameManager.instance.enemies[enemyRefId].GetComponent<EnemyMovement>().ReceiveMove(position);
        }   
    }

    public static void TakeDamage(Packet _packet) {
        int characterType = _packet.ReadInt();
        string characterRefId = _packet.ReadString();
        float damageTaken = _packet.ReadFloat();
        if (characterType == (int) CharacterType.Player) {
            if (GameManager.instance.players.ContainsKey(characterRefId)) {
                GameManager.instance.players[characterRefId].GetComponent<PlayerStatsManager>().ReceiveTakeDamage(damageTaken);
            }
        } else if (characterType == (int) CharacterType.Enemy) {
            if (GameManager.instance.enemies.ContainsKey(characterRefId)) {
                GameManager.instance.enemies[characterRefId].GetComponent<EnemyStatsManager>().ReceiveTakeDamage(damageTaken);
            }
        }
    }

    public static void Die(Packet _packet) {
        int characterType = _packet.ReadInt();
        string characterRefId = _packet.ReadString();
        if (characterType == (int) CharacterType.Player) {
            if (GameManager.instance.players.ContainsKey(characterRefId)) {
                GameManager.instance.players[characterRefId].GetComponent<PlayerStatsManager>().ReceiveDie();
            }
        } else if (characterType == (int) CharacterType.Enemy) {
            if (GameManager.instance.enemies.ContainsKey(characterRefId)) {
                GameManager.instance.enemies[characterRefId].GetComponent<EnemyStatsManager>().ReceiveDie();
            }
        }
    }
}
