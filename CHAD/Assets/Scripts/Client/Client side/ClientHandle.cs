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

        ClientSend.SPAM();
        PlayerClient.instance.udp.Connect(
            ((IPEndPoint)PlayerClient.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void PlayerPosition(Packet _packet)
    {
        Vector2 position = _packet.ReadVector2();
        //TODO: Send info to GameManager
    }

    public static void SpawnPlayer(Packet _packet)
    {
        Debug.Log("CLIENT RECEIVED SPAWN DATA");
        int playerIdReceived = _packet.ReadInt();
        Vector2 position = _packet.ReadVector2();
        int characterType = _packet.ReadInt();
        GameManager.instance.SpawnPlayer(playerIdReceived, characterType, position, true);
    }
}
