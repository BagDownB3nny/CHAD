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
        Client.instance.myId = _id;

        ClientSend.WelcomeReceived();
        Client.instance.udp.Connect(
            ((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }
}
