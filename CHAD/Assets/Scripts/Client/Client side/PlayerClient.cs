using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class PlayerClient : MonoBehaviour
{
    public static PlayerClient instance;
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 26950;
    public int myId = 0;
    public ClientTCP tcp;
    public ClientUDP udp;
    public GameObject player;
    public bool isConnected = false;

    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void SetServerIp(string _serverIp) {
        ip = _serverIp;
    }

    public void ConnectToServer()
    {
        InitializeClientData();
        tcp = new ClientTCP();
        udp = new ClientUDP();
        isConnected = true;
        tcp.Connect();
    }

    public class ClientTCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }

        public void Disconnect() {
            instance.Disconnect();
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();
            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
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
                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    instance.Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);
                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
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
                        packetHandlers[_packetId](_packet);
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
    }

    public class ClientUDP
    {
        public UdpClient socket;
        public IPEndPoint endPoint;

        public ClientUDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
        }

        public void Connect(int _localPort)
        {
            socket = new UdpClient(_localPort);

            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallback, null);

            using (Packet _packet = new Packet())
            {
                SendData(_packet);
            }
        }

        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(instance.myId);
                if (socket != null)
                {
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via UDP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                if (_data.Length < 4)
                {
                    instance.Disconnect();
                    return;
                }

                HandleData(_data);
            }
            catch
            {
                Disconnect();
            }
        }

        private void HandleData(byte[] _data)
        {
            using (Packet _packet = new Packet(_data))
            {
                int _packetLength = _packet.ReadInt();
                _data = _packet.ReadBytes(_packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_data))
                {
                    int _packetId = _packet.ReadInt();
                    packetHandlers[_packetId](_packet);
                }
            });
        }

        public void Disconnect() {
            instance.Disconnect();

            endPoint = null;
            socket = null;
        }
    }

    void OnApplicationQuit()
    {
        Disconnect();
    }

    public void Disconnect() {
        if (isConnected) {
            Debug.Log("PlayerClient is disconnecting");
            isConnected = false;
            tcp.socket.Close();
            udp.socket.Close();
        }
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int)ServerPackets.welcome, ClientHandle.Welcome },
            {(int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer},
            {(int)ServerPackets.movePlayer, ClientHandle.MovePlayer},
            {(int)ServerPackets.rangedAttack, ClientHandle.RangedAttack},
            {(int)ServerPackets.spawnEnemy, ClientHandle.SpawnEnemy},
            {(int)ServerPackets.moveEnemy, ClientHandle.MoveEnemy},
            {(int)ServerPackets.takeDamage, ClientHandle.TakeDamage},
            {(int)ServerPackets.die, ClientHandle.Die},
            {(int)ServerPackets.moveProjectile, ClientHandle.MoveProjectile},
            {(int)ServerPackets.destroyProjectile, ClientHandle.DestroyProjectile},
            {(int)ServerPackets.meleeAttack, ClientHandle.MeleeAttack},
            {(int)ServerPackets.rotateRangedWeapon, ClientHandle.RotateRangedWeapon},
            {(int)ServerPackets.removePlayer, ClientHandle.RemovePlayer},
            {(int)ServerPackets.destroyDamageDealer, ClientHandle.DestroyDamageDealer},
            {(int)ServerPackets.readyStatus, ClientHandle.ReadyStatus},
            {(int)ServerPackets.changeClass, ClientHandle.ChangeClass},
            {(int)ServerPackets.broadcast, ClientHandle.Broadcast},
            {(int)ServerPackets.equipGun, ClientHandle.EquipGun},
            {(int)ServerPackets.loadLobby, ClientHandle.LoadLobby},
            {(int)ServerPackets.loadEmptyMap, ClientHandle.LoadEmptyMap},
            {(int)ServerPackets.loadMap, ClientHandle.LoadMap},
            {(int)ServerPackets.addGun, ClientHandle.AddGun},
            {(int)ServerPackets.updateEnemySpawnerStats, ClientHandle.UpdateEnemySpawnerStats},
            {(int)ServerPackets.weaponDrop, ClientHandle.WeaponDrop},
            {(int)ServerPackets.removeWeaponDrop, ClientHandle.RemoveWeaponDrop},
            {(int)ServerPackets.addItem, ClientHandle.AddItem},
            {(int)ServerPackets.itemDrop, ClientHandle.ItemDrop},
            {(int)ServerPackets.removeItemDrop, ClientHandle.RemoveItemDrop},
            {(int)ServerPackets.setBossAttack, ClientHandle.SetBossAttack},
            {(int)ServerPackets.moveBossAttack, ClientHandle.MoveBossAttack},
            {(int)ServerPackets.moveBoss, ClientHandle.MoveBoss},
            {(int)ServerPackets.spawnBoss, ClientHandle.SpawnBoss},
            {(int)ServerPackets.endBossAttack, ClientHandle.EndBossAttack}
        };
    }


}