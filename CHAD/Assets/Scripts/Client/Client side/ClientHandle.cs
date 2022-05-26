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
        if (GameManager.instance.players.ContainsKey(_affectedPlayerId) &&
                GameManager.instance.players[_affectedPlayerId]) {
            GameManager.instance.players[_affectedPlayerId]
                    .GetComponent<PlayerMovement>().ReceiveMovement(_position);
        }
    }

    public static void PlayerAttack(Packet _packet) {
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

    public static void ProjectileMovement(Packet _packet) {
        int _projectileId = _packet.ReadInt();
        Vector2 _position  = _packet.ReadVector2();
        if (GameManager.instance.projectiles.ContainsKey(_projectileId)) {
            GameManager.instance.projectiles[_projectileId].GetComponent<ProjectileMovement>()
            .ReceiveMovement(_position);
        }
    }

    public static void DestroyProjectile(Packet _packet) {
        int _projectileId = _packet.ReadInt();
        GameManager.instance.projectiles[_projectileId].GetComponent<ProjectileMovement>()
            .ReceiveDestroyProjectile();
    }
    
    public static void MoveEnemy(Packet _packet) {
        int enemyRefId = _packet.ReadInt();
        Vector2 position = _packet.ReadVector2();
        GameManager.instance.enemies[enemyRefId].transform.position = position;
    }

    public static void DisconnectPlayer(Packet _packet) {
        int playerRefId = _packet.ReadInt();
        Destroy(GameManager.instance.players[playerRefId]);
        GameManager.instance.players.Remove(playerRefId);
    }
}
