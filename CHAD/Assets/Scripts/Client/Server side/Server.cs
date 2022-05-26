using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Server
{

    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }

    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static Dictionary<int, ServerClient> serverClients = new Dictionary<int, ServerClient>();

    public delegate void PacketHandler(int _fromClient, Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    public static void Start(int _maxPlayers, int _port)
    {
        Debug.Log("Starting Server...");
        MaxPlayers = _maxPlayers;
        Port = _port;

        InitializeServerData();

        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(UDPReceiveCallback, null);
        Debug.Log($"Server started on port {Port}");

    }

    public static void UDPReceiveCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (_data.Length < 4)
            {
                return;
            }
            using (Packet _packet = new Packet(_data))
            {
                int _clientId = _packet.ReadInt();
                if (_clientId == 0)
                {
                    return;
                }
                if (serverClients[_clientId].udp.endPoint == null)
                {
                    serverClients[_clientId].udp.Connect(_clientEndPoint);
                    return;
                }
                if (serverClients[_clientId].udp.endPoint.ToString() ==
                    _clientEndPoint.ToString())
                {
                    //Debug.Log(_clientId);
                    serverClients[_clientId].udp.HandleData(_packet);
                }

            }

        }
        catch (Exception _ex)
        {
            Debug.Log($"Failed to receive UDP data: {_ex}");
        }
    }

    public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
    {
        try
        {
            if (_clientEndPoint != null)
            {
                udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error sending UDP data: {_ex}");
        }
    }

    private static void TCPConnectCallback(IAsyncResult _result)
    {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        Debug.Log($"Incoming connection from {_client.Client.RemoteEndPoint}");
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);


        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (serverClients[i].tcp.socket == null)
            {
                serverClients[i].tcp.Connect(_client);
                return;
            }
        }
    }

    public static void InitializeServerData()
    {
        for (int i = 0; i <= MaxPlayers; i++)
        {
            serverClients.Add(i, new ServerClient(i));
        }
        packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived},
                //{(int)ClientPackets.SPAM, ServerHandle.SPAM},
                {(int)ClientPackets.spawnPlayer, ServerHandle.SpawnPlayer},
                {(int)ClientPackets.movePlayer, ServerHandle.MovePlayer},
                {(int)ClientPackets.rotateGun, ServerHandle.RotateGun},
                {(int)ClientPackets.rangedAttack, ServerHandle.RangedAttack}

            };
    }
}
