using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    public static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    public static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i <= Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp != null)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    public static void SendTCPDataToAll(int _noSend, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i <= Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp != null && i != _noSend)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    public static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    public static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i <= Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp != null)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    public static void SendUDPDataToAll(int _noSend, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i <= Server.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp != null && i != _noSend)
            {
                Server.clients[i].udp.SendData(_packet);
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

    public static void SpawnPlayer(int _toClient, GameObject _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);
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
