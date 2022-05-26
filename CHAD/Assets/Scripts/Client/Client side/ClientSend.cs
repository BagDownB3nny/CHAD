using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    public static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        PlayerClient.instance.tcp.SendData(_packet);
    }

    public static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        PlayerClient.instance.udp.SendData(_packet);
    }

    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(PlayerClient.instance.myId);
            _packet.Write($"Player {PlayerClient.instance.myId}");
            //_packet.Write(UI_Manager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void MovePlayer(bool[] _input)
    {
        using (Packet _packet = new Packet((int)ClientPackets.movePlayer))
        {
            for (int i = 0; i < _input.Length; i++)
            {
                _packet.Write(_input[i]);
            }
            SendUDPData(_packet);
        }
    }

    /*
    public static void SPAM()
    {
        using (Packet _packet = new Packet((int)ClientPackets.SPAM))
        {
            _packet.Write("SPAM");
            SendTCPData(_packet);
        }
    }*/

    public static void SpawnPlayer(int _characterType, Vector2 position)
    {
        using (Packet _packet = new Packet((int)ClientPackets.spawnPlayer))
        {
            _packet.Write(_characterType);
            _packet.Write(position);
            SendTCPData(_packet);
        }
    }

    public static void RotateRangedWeapon(float _rotation)
    {
        using (Packet _packet = new Packet((int)ClientPackets.rotateGun))
        {
            _packet.Write(_rotation);
            SendUDPData(_packet);
        }
    }

    public static void RangedAttack(string _characterRefId) {
        using (Packet _packet = new Packet((int)ClientPackets.rangedAttack)) {
            _packet.Write(_characterRefId);
            SendTCPData(_packet);
        }
    }
}
