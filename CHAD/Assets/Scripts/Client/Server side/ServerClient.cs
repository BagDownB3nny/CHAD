using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;


public class ServerClient
{
    public static int dataBufferSize = 4096;

    public int id;
    public ServerTCP tcp;
    public ServerUDP udp;
    public GameObject player;

    public ServerClient(int _clientId)
    {
        id = _clientId;
        tcp = new ServerTCP(id);
        udp = new ServerUDP(id);
    }

    public class ServerTCP
    {
        public TcpClient socket;

        private readonly int id;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public ServerTCP(int _id)
        {
            id = _id;
        }

        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream();

            receivedData = new Packet();
            receiveBuffer = new byte[dataBufferSize];

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            ServerSend.Welcome(id, "Welcome to the server!");
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to player {id} via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                Debug.Log("Received some TCP data!");
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    Server.serverClients[id].Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error receiving TCP data: {_ex}");
                Server.serverClients[id].Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            Debug.Log("Handling data");
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        Server.packetHandlers[_packetId](id, _packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        public void Disconnect() {
            socket.Close();
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    public class ServerUDP
    {
        public IPEndPoint endPoint;

        private int id;

        public ServerUDP(int _id)
        {
            id = _id;
        }

        public void Connect(IPEndPoint _endPoint)
        {
            Debug.Log("Server UDP connected to client");
            endPoint = _endPoint;
        }

        public void SendData(Packet _packet)
        {
            Server.SendUDPData(endPoint, _packet);
        }

        public void HandleData(Packet _packetData)
        {
            int _packetLength = _packetData.ReadInt();
            byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_packetBytes))
                {
                    int _packetId = _packet.ReadInt();
                    Server.packetHandlers[_packetId](id, _packet);
                }
            });
        }

        public void Disconnect() {
            endPoint = null;
        }
    }

    private void Disconnect() {
        tcp.Disconnect();
        udp.Disconnect();
        GameManager.instance.Disconnect(id);
    }

    public void SendIntoGame(int _characterType, Vector2 position)
    {
        GameManager.instance.SpawnPlayer(id.ToString(), _characterType, position);
        player = GameManager.instance.players[id.ToString()];
        foreach (ServerClient _client in Server.serverClients.Values)
        {
            if (_client.player != null)
            {
                GameObject character = GameManager.instance.players[_client.id.ToString()];
                Debug.Log("ServerClient telling client to spawn character of type " + character.GetComponent<PlayerStatsManager>().characterClass);
                ServerSend.SpawnPlayer(id, _client.id, character, character.GetComponent<PlayerStatsManager>().characterClass);
            }
        }
        foreach (ServerClient _client in Server.serverClients.Values)
        {
            if (_client.player != null)
            {
                if (_client.id != id)
                {
                    ServerSend.SpawnPlayer(_client.id, id, player, _characterType);
                }
            }
        }
    }
}
