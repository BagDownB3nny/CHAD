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

        PlayerClient.instance.udp.Connect(
            ((IPEndPoint)PlayerClient.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        Debug.Log("CLIENT RECEIVED SPAWN DATA");
        int playerIdReceived = _packet.ReadInt();
        Vector2 position = _packet.ReadVector2();
        int characterType = _packet.ReadInt();
        GameManager.instance.SpawnPlayer(playerIdReceived, characterType, position, true);
    }

    public static void MovePlayer(Packet _packet)
    {
        int _affectedPlayerId = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();
        GameManager.instance.players[_affectedPlayerId].GetComponent<PlayerMovement>().ReceiveMovement(_position);
    }
}
