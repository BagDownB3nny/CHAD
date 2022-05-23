using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.serverClients[_fromClient].tcp.socket.Client.RemoteEndPoint} " +
            $"successfully connected and is now player {_clientIdCheck}");
        if (_clientIdCheck != _fromClient)
        {
            Debug.Log($"Player \"{_username}\" (Id: {_fromClient}) " +
               $"has assumed the wrong Id ({_clientIdCheck})!");
        }
        Debug.Log("Welcome received!");
    }

    public static void MovePlayer(int _fromClient, Packet _packet)
    {
        bool[] _input = new bool[4];
        for (int i = 0; i < _input.Length; i++)
        {
            _input[i] = _packet.ReadBool();
        }
        
        Vector2 _position = GameManager.instance.players[_fromClient].GetComponent<PlayerMovement>().MovePlayer(_input);
        ServerSend.MovePlayer(_fromClient, _position);
    }

    /*
    public static void SPAM(int _fromClient, Packet _packet)
    {
        Debug.Log(_packet.ReadString());
    }*/

    public static void SpawnPlayer(int _fromClient, Packet _packet) {
        int characterType = _packet.ReadInt();
        Vector2 position = _packet.ReadVector2();
        Server.serverClients[_fromClient].SendIntoGame(characterType, position);
    }

    public static void ReceiveGunRotation(int _fromClient, Packet _packet)
    {
        Quaternion rotation = _packet.ReadQuaternion();
        Server.serverClients[_fromClient].player.GetComponent<PlayerStatsManager>().
            weaponsManagerScipt.currentWeapon.GetComponent<PlayerRangedWeapon>().ReceiveGunRotation(rotation);
    }

    public static void Attack(int _fromClient, Packet _packet) {
        Debug.Log("ServerHandle receives attack");
        PlayerWeapons gunType = (PlayerWeapons) _packet.ReadInt();
        float directionRotation = _packet.ReadFloat();
        GameManager.instance.PlayerAttack(_fromClient, gunType, directionRotation);
    }

}
