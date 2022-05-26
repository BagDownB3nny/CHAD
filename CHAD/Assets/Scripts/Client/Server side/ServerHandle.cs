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

    public static void ReceiveGunRotation(int _fromClient, Packet _packet)
    {
        Quaternion rotation = _packet.ReadQuaternion();
        Server.serverClients[_fromClient].player.GetComponent<PlayerWeaponsManager>()
            .currentWeapon.GetComponent<PlayerRangedWeapon>().ReceiveGunRotation(rotation);
    }

    public static void PlayerAttack(int _fromClient, Packet _packet) {
        PlayerWeapons gunType = (PlayerWeapons) _packet.ReadInt();
        float directionRotation = _packet.ReadFloat();

        //perform the attack on server side and receive the projectile info
        object[] projectileInfo = GameManager.instance.players[_fromClient.ToString()].GetComponent<PlayerWeaponsManager>().weaponScript
                .Attack(gunType, directionRotation);

        if (projectileInfo != null) {
            GameObject bullet = (GameObject) projectileInfo[0];
            //set projectile reference id  and add it to dictionary on server side
            //string projectileRefId = string.Format("P{0}")
            bullet.GetComponent<ProjectileStatsManager>().projectileRefId = GameManager.instance.projectileRefId;
            GameManager.instance.projectiles.Add(GameManager.instance.projectileRefId, bullet);
            //send attack info to client
            ServerSend.PlayerAttack(_fromClient, GameManager.instance.projectileRefId, gunType, (float) projectileInfo[1]);
            GameManager.instance.projectileRefId++;
        }
    }
}
