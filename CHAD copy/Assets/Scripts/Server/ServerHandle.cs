using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} " +
            $"successfully connected and is now player {_clientIdCheck}");
        if (_clientIdCheck != _fromClient)
        {
            Debug.Log($"Player \"{_username}\" (Id: {_fromClient}) " +
               $"has assumed the wrong Id ({_clientIdCheck})!");
        }
        Debug.Log("Welcome received!");
        Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        Debug.Log("Movement received");
        bool[] _inputs = new bool[4];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Server.clients[_fromClient].PlayerMovement(_inputs);

    }

    public static void SPAM(int _fromClient, Packet _packet)
    {
        Debug.Log(_packet.ReadString());
    }

}
