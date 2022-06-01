using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    public List<bool> ready = new List<bool>();
    private bool myReadyStatus = false;

    private void Awake() {
        instance = this;
    }

    public void ToggleReadyStatus() {
        if (myReadyStatus) {
            myReadyStatus = false;
        } else {
            myReadyStatus = true;
        }
        SendReadyStatus();
    }

    public void ReadyStatus(int _playerRefId, bool _readyStatus) {
        ready[_playerRefId] = _readyStatus;
        ServerSend.ReadyStatus(_playerRefId, _readyStatus);
    }

    public void SendReadyStatus() {
        ClientSend.ReadyStatus(PlayerClient.instance.myId, myReadyStatus);
    }

    public void ReceiveReadyStatus(int _playerRefId, bool _readyStatus) {
        ready[_playerRefId] = _readyStatus;
    }
}
