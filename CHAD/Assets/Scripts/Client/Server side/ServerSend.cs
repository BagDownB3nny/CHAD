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
        for (int i = 1; i <= Server.MaxPlayers; i++)
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
        for (int i = 1; i <= Server.MaxPlayers; i++)
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
        for (int i = 1; i <= Server.MaxPlayers; i++)
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
        for (int i = 1; i <= Server.MaxPlayers; i++)
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
            _packet.Write((Vector2) _player.transform.position);
            _packet.Write(characterType);
            Debug.Log("Server send packet: " + _affectedPlayerId + _player.transform.position + characterType);
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

    public static void RangedAttack(CharacterType _characterType, string _affectedCharacterRefId, 
            string _projectileRefId, float _projectileDirectionRotation) {
        using (Packet _packet = new Packet((int)ServerPackets.rangedAttack))
        {
            _packet.Write((int)_characterType);
            _packet.Write(_affectedCharacterRefId);
            _packet.Write(_projectileRefId);
            _packet.Write(_projectileDirectionRotation);
            SendTCPDataToAll(_packet);
        }
    }

    public static void MeleeAttack(CharacterType _characterType, string _affectedCharacterRefId, 
            string _damageDealerRefId) {
        using (Packet _packet = new Packet((int)ServerPackets.meleeAttack))
        {
            _packet.Write((int)_characterType);
            _packet.Write(_affectedCharacterRefId);
            _packet.Write(_damageDealerRefId);
            SendTCPDataToAll(_packet);
        }
    }

    public static void SpawnEnemy(string _spawnerRefId, string _enemyRefId, int _enemyId, Vector2 _position) {
        using (Packet _packet = new Packet((int)ServerPackets.spawnEnemy))
        {
            _packet.Write(_spawnerRefId);
            _packet.Write(_enemyRefId);
            _packet.Write(_enemyId);
            _packet.Write(_position);
            SendTCPDataToAll(_packet);
        }
    }

    public static void MoveProjectile(string _projectileRefId, Vector2 _position) {
        using (Packet _packet = new Packet((int)ServerPackets.moveProjectile))
        {
            _packet.Write(_projectileRefId);
            _packet.Write(_position);
            SendUDPDataToAll(_packet);
        }
    }

    public static void DestroyProjectile(string _projectileRefId) {
        using (Packet _packet = new Packet((int)ServerPackets.destroyProjectile))
        {
            _packet.Write(_projectileRefId);
            SendTCPDataToAll(_packet);
        }
    }

    public static void DestroyDamageDealer(string _damageDealerRefId) {
        using (Packet _packet = new Packet((int)ServerPackets.destroyDamageDealer))
        {
            _packet.Write(_damageDealerRefId);
            SendTCPDataToAll(_packet);
        }
    }

    public static void MoveEnemy(string _enemyRefId, Vector2 _position) {
        using (Packet _packet = new Packet((int)ServerPackets.moveEnemy))
        {
            _packet.Write(_enemyRefId);
            _packet.Write(_position);
            SendUDPDataToAll(_packet);
        }
    }

    public static void TakeDamage(int _characterType, string _characterRefId, float _damageTaken) {
        using (Packet _packet = new Packet((int)ServerPackets.takeDamage))
        {
            _packet.Write(_characterType);
            _packet.Write(_characterRefId);
            _packet.Write(_damageTaken);
            SendTCPDataToAll(_packet);
        }
    }

    public static void Die(int _characterType, string _characterRefId) {
        using (Packet _packet = new Packet((int)ServerPackets.die))
        {
            _packet.Write(_characterType);
            _packet.Write(_characterRefId);
            SendTCPDataToAll(_packet);
        }
    }

    public static void RemovePlayer(string _playerRefId) {
        using (Packet _packet = new Packet((int)ServerPackets.removePlayer))
        {
            _packet.Write(_playerRefId);
            SendTCPDataToAll(_packet);
        }
    }
    public static void RotateRangedWeapon(CharacterType _characterType, string _characterRefId, float _directionRotation) {
        using (Packet _packet = new Packet((int)ServerPackets.rotateRangedWeapon))
        {
            _packet.Write((int) _characterType);
            _packet.Write(_characterRefId);
            _packet.Write(_directionRotation);
            SendUDPDataToAll(_packet);
        }
    }

    public static void RotatePlayerRangedWeapon(int _noSend, string _characterRefId, float _directionRotation) {
        using (Packet _packet = new Packet((int)ServerPackets.rotateRangedWeapon))
        {
            _packet.Write((int) CharacterType.Player);
            _packet.Write(_characterRefId);
            _packet.Write(_directionRotation);
            SendUDPDataToAll(_noSend, _packet);
        }
    }

    public static void ReadyStatus(int _playerRefId, bool _readyStatus) {
        using (Packet _packet = new Packet((int)ServerPackets.readyStatus))
        {
            _packet.Write(_playerRefId);
            _packet.Write(_readyStatus);
            SendTCPDataToAll(_packet);
        }
    }

    public static void ChangeClass(int _playerRefId, int _playerClass, Vector2 _playerPosition) {
        using (Packet _packet = new Packet((int)ServerPackets.changeClass))
        {
            _packet.Write(_playerRefId);
            _packet.Write(_playerClass);
            _packet.Write(_playerPosition);
            SendTCPDataToAll(_packet);
        }
    }

    public static void Broadcast(string _msg) {
        using (Packet _packet = new Packet((int)ServerPackets.broadcast))
        {
            _packet.Write(_msg);
            SendTCPDataToAll(_packet);
        }
    }

    public static void EquipGun(int _noSend, int _gunIndex) {
        using (Packet _packet = new Packet((int)ServerPackets.equipGun))
        {
            _packet.Write(_gunIndex);
            _packet.Write(_noSend);
            SendTCPDataToAll(_noSend, _packet);
        }
    }
}
