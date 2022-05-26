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
        
        Vector2 _position = GameManager.instance.players[_fromClient.ToString()].GetComponent<PlayerMovement>().MovePlayer(_input);
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

    public static void RotateRangedWeapon(int _fromClient, Packet _packet)
    {
        string affectedCharacterRefId = _packet.ReadString();
        float directionRotation = _packet.ReadFloat();
        if (GameManager.instance.players.ContainsKey(affectedCharacterRefId)) {
            GameManager.instance.players[affectedCharacterRefId].GetComponent<PlayerWeaponsManager>().weaponScript
                    .ReceiveRotateRangedWeapon(directionRotation);
        }
        //relay the weapon rotation from client to all other clients
        ServerSend.RotatePlayerRangedWeapon(_fromClient, affectedCharacterRefId, directionRotation);
    }

    public static void RangedAttack(int _fromClient, Packet _packet) {
        string characterRefId =  _packet.ReadString();
        GameManager.instance.players[characterRefId].GetComponent<PlayerWeaponsManager>()
                .weaponScript.Attack();
    }
}
