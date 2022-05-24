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
        GameManager.instance.SpawnPlayer(playerIdReceived, characterType, position, true);
    }

    public static void MovePlayer(Packet _packet)
    {
        int _affectedPlayerId = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();
        GameManager.instance.players[_affectedPlayerId].GetComponent<PlayerMovement>().ReceiveMovement(_position);
    }

    public static void ReceivePlayerAttack(Packet _packet) {
        int _affectedPlayerId = _packet.ReadInt();
        int _projectileRefId = _packet.ReadInt();
        PlayerWeapons gunType = (PlayerWeapons) _packet.ReadInt();
        float bulletDirectionRotation = _packet.ReadFloat();
        GameManager.instance.ReceivePlayerAttack(_affectedPlayerId, _projectileRefId, gunType, bulletDirectionRotation);
    }

    public static void SpawnEnemy(Packet _packet) {
        int enemyRefId = _packet.ReadInt();
        int enemyId = _packet.ReadInt();
        Vector2 position = _packet.ReadVector2();
        GameManager.instance.ReceiveSpawnEnemy(enemyRefId, enemyId, position);
    }

    public static void ReceiveProjectileMovement(Packet _packet) {
        int _projectileId = _packet.ReadInt();
        Vector2 _position  = _packet.ReadVector2();
        if (GameManager.instance.projectiles.ContainsKey(_projectileId)) {
            GameManager.instance.projectiles[_projectileId].GetComponent<ProjectileMovement>()
            .ReceiveMovement(_position);
        }
    }

    public static void ReceiveDestroyProjectile(Packet _packet) {
        int _projectileId = _packet.ReadInt();
        GameManager.instance.projectiles[_projectileId].GetComponent<ProjectileMovement>()
            .ReceiveDestroyProjectile();
    }
    
    public static void MoveEnemy(Packet _packet) {
        int enemyRefId = _packet.ReadInt();
        Vector2 position = _packet.ReadVector2();
        GameManager.instance.enemies[enemyRefId].GetComponent<EnemyMovement>().ReceiveMove(position);
    }

    public static void TakeDamage(Packet _packet) {
        int characterType = _packet.ReadInt();
        int characterRefId = _packet.ReadInt();
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
        int characterRefId = _packet.ReadInt();
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
