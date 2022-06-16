using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static bool IsPresent(Dictionary<string, GameObject> dict, string refId) {
        return dict.ContainsKey(refId) && dict[refId] != null;
    }
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
        if (IsPresent(GameManager.instance.players, _fromClient.ToString())) {
            Vector2 _position = GameManager.instance.players[_fromClient.ToString()].GetComponent<PlayerMovement>().MovePlayer(_input);
            ServerSend.MovePlayer(_fromClient, _position);
        }
    }

    public static void RotateRangedWeapon(int _fromClient, Packet _packet)
    {
        string affectedCharacterRefId = _packet.ReadString();
        float directionRotation = _packet.ReadFloat();
        if (IsPresent(GameManager.instance.players, affectedCharacterRefId)) {
            if (GameManager.instance.players[affectedCharacterRefId].GetComponent<PlayerWeaponsManager>().weaponScript != null) {
                GameManager.instance.players[affectedCharacterRefId].GetComponent<PlayerWeaponsManager>().weaponScript
                        .ReceiveRotateRangedWeapon(directionRotation);
            }
        }
        //relay the weapon rotation from client to all other clients
        ServerSend.RotatePlayerRangedWeapon(_fromClient, affectedCharacterRefId, directionRotation);
    }

    public static void RangedAttack(int _fromClient, Packet _packet) {
        string characterRefId =  _packet.ReadString();
        if (IsPresent(GameManager.instance.players, characterRefId)) {
            GameManager.instance.players[characterRefId].GetComponent<PlayerWeaponsManager>()
                    .weaponScript.Attack();
        }
    }

    public static void ReadyStatus(int _fromClient, Packet _packet) {
        int playerRefId = _packet.ReadInt();
        bool readyStatus = _packet.ReadBool();
        LobbyManager.instance.ReadyStatus(playerRefId.ToString(), readyStatus);
    }

    public static void ChangeClass(int _fromClient, Packet _packet) {
        PlayerClasses playerClass = (PlayerClasses)_packet.ReadInt();
        GameManager.instance.playerSpawner.SpawnPlayer(_fromClient, playerClass);
        foreach (ServerClient serverClient in Server.serverClients.Values) {
            ServerSend.SpawnPlayer(serverClient.id, _fromClient, playerClass);
        }
    }

    public static void EquipGun(int _fromClient, Packet _packet) {
        int gunIndex = _packet.ReadInt();
        GameManager.instance.players[_fromClient.ToString()].GetComponent<PlayerWeaponsManager>()
                .ReceiveEquipGun(gunIndex);
        foreach (ServerClient serverClient in Server.serverClients.Values)
        {
            if (serverClient.id != _fromClient)
            {
                ServerSend.EquipGun(serverClient.id, _fromClient, gunIndex);
            }
        }
    }

    public static void MapLoaded(int _fromClient, Packet _packet)
    {
        Server.serverClients[_fromClient].spawnedIn = true;
        GameManager.instance.playerSpawner.SpawnPlayer(_fromClient, PlayerClasses.Captain);
        foreach (ServerClient serverClient in Server.serverClients.Values)
        {
            if (serverClient.spawnedIn)
            {
                // Telling all clients to spawn in this player
                ServerSend.SpawnPlayer(serverClient.id, _fromClient,
                        PlayerInfoManager.AllPlayerInfo[_fromClient.ToString()].playerClass);
                if (serverClient.id != _fromClient)
                {
                    // Telling this client to spawn in all players (except itself)
                    ServerSend.SpawnPlayer(_fromClient, serverClient.id,
                            PlayerInfoManager.AllPlayerInfo[serverClient.id.ToString()].playerClass);
                    // Telling this client to equip guns for all players
                    ServerSend.EquipGun(_fromClient, serverClient.id,
                            GameManager.instance.players[serverClient.id.ToString()].GetComponent<PlayerWeaponsManager>().currentWeaponId);
                }
            }
        }
    }
}
