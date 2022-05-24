using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    public static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.serverClients[_toClient].tcp.SendData(_packet);
    }

    public static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i <= Server.MaxPlayers; i++)
        {
            if (Server.serverClients[i].tcp != null)
            {
                Server.serverClients[i].tcp.SendData(_packet);
            }
        }
    }

    public static void SendTCPDataToAll(int _noSend, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i <= Server.MaxPlayers; i++)
        {
            if (Server.serverClients[i].tcp != null && i != _noSend)
            {
                Server.serverClients[i].tcp.SendData(_packet);
            }
        }
    }

    public static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.serverClients[_toClient].udp.SendData(_packet);
    }

    public static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i <= Server.MaxPlayers; i++)
        {
            if (Server.serverClients[i].tcp != null)
            {
                Server.serverClients[i].udp.SendData(_packet);
            }
        }
    }

    public static void SendUDPDataToAll(int _noSend, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i <= Server.MaxPlayers; i++)
        {
            if (Server.serverClients[i].tcp != null && i != _noSend)
            {
                Server.serverClients[i].udp.SendData(_packet);
            }
        }
    }

    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_toClient);
            _packet.Write(_msg);
            SendTCPData(_toClient, _packet);
        }
    }

    public static void SpawnPlayer(int _toClient, int _affectedPlayerId, GameObject _player, int characterType)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_affectedPlayerId);
            _packet.Write(_player.transform.position);
            _packet.Write(characterType);
            SendTCPData(_toClient, _packet);
        }
    }

    public static void MovePlayer(int _affectedPlayerId, Vector2 _position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.movePlayer))
        {
            _packet.Write(_affectedPlayerId);
            _packet.Write(_position);
            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerAttack(int _affectedPlayerId, int _projectileId, 
        PlayerWeapons gunType, float directionRotation) {
        using (Packet _packet = new Packet((int)ServerPackets.playerAttack))
        {
            Debug.Log("Server sends attack");
            _packet.Write(_affectedPlayerId);
            _packet.Write(_projectileId);
            _packet.Write((int) gunType);
            _packet.Write(directionRotation);
            SendTCPDataToAll(_packet);
        }
    }

    public static void SpawnEnemy(int _enemyRefId, int _enemyId, Vector2 _position) {
        using (Packet _packet = new Packet((int)ServerPackets.spawnEnemy))
        {
            _packet.Write(_enemyRefId);
            _packet.Write(_enemyId);
            _packet.Write(_position);
            SendTCPDataToAll(_packet);
        }
    }

    public static void MoveEnemy(int _enemyRefId, Vector2 _position) {
        using (Packet _packet = new Packet((int)ServerPackets.moveEnemy))
        {
            _packet.Write(_enemyRefId);
            _packet.Write(_position);
            SendUDPDataToAll(_packet);
        }
    }

    public static void TakeDamage(int _characterType, int _characterRefId, float _damageTaken) {
        using (Packet _packet = new Packet((int)ServerPackets.takeDamage))
        {
            _packet.Write(_characterType);
            _packet.Write(_characterRefId);
            _packet.Write(_damageTaken);
            SendTCPDataToAll(_packet);
        }
    }

    public static void Die(int _characterType, int _characterRefId) {
        using (Packet _packet = new Packet((int)ServerPackets.die))
        {
            _packet.Write(_characterType);
            _packet.Write(_characterRefId);
            SendTCPDataToAll(_packet);
        }
    }
}
