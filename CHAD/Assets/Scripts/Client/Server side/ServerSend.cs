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

    public static void SpawnPlayer(int _toClient, int playerIdToSend, GameObject _player, int characterType)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(playerIdToSend);
            _packet.Write(_player.transform.position);
            _packet.Write(characterType);
            SendTCPData(_toClient, _packet);
        }
    }

    public static void PlayerPosition(int _toClient, Vector2 _position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_position);
            SendUDPData(_toClient, _packet);
        }
    }
}
