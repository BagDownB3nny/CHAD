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

    public static void RotateRangedWeapon(string _characterRefId, float _rotation)
    {
        using (Packet _packet = new Packet((int)ClientPackets.rotateRangedWeapon))
        {
            _packet.Write(_characterRefId);
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

    public static void ReadyStatus(int _playerRefId, bool _readyStatus) {
        using (Packet _packet = new Packet((int)ClientPackets.readyStatus)) {
            _packet.Write(_playerRefId);
            _packet.Write(_readyStatus);
            SendTCPData(_packet);
        }
    }

    public static void ChangeClass(int _playerClass) {
        using (Packet _packet = new Packet((int)ClientPackets.changeClass)) {
            _packet.Write(_playerClass);
            SendTCPData(_packet);
        }
    }

    public static void EquipGun(int _index) {
        using (Packet _packet = new Packet((int)ClientPackets.equipGun)) {
            _packet.Write(_index);
            SendTCPData(_packet);
        }
    }

    public static void LobbyLoaded()
    {
        using (Packet _packet = new Packet((int)ClientPackets.lobbyLoaded))
        {
            SendTCPData(_packet);
        }
    }

    public static void EmptyMapLoaded()
    {
        using (Packet _packet = new Packet((int)ClientPackets.emptyMapLoaded))
        {
            SendTCPData(_packet);
        }
    }

    public static void MapLoaded()
    {
        using (Packet _packet = new Packet((int)ClientPackets.mapLoaded))
        {
            SendTCPData(_packet);
        }
    }

    public static void Interact()
    {
        using (Packet _packet = new Packet((int)ClientPackets.interact))
        {
            SendTCPData(_packet);
        }
    }
}
