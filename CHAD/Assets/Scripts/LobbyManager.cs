using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    public Dictionary<int, bool> ready = new Dictionary<int, bool>();
    private bool myReadyStatus = false;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }
    }

    public void SpawnPlayer(int _playerId) {
        ready.Add(_playerId, false);
    }

    public void ReceiveSpawnPlayer(int _playerId) {
        ready.Add(_playerId, false);
    }
    public void ToggleReadyStatus(bool toggle) {
        myReadyStatus = toggle;
        SendReadyStatus();
    }

    public void ReadyStatus(int _playerRefId, bool _readyStatus) {
        ready[_playerRefId] = _readyStatus;
        foreach (KeyValuePair<int, bool> readyStatus in ready) {
            if (!readyStatus.Value) {
                ServerSend.ReadyStatus(_playerRefId, _readyStatus);
                return;
            } 
        }
        if (ready.Count > 0) {
            ServerSend.Broadcast("ALL HAVE READIED");
        }
    }

    public void SendReadyStatus() {
        ClientSend.ReadyStatus(PlayerClient.instance.myId, myReadyStatus);
    }

    public void ReceiveReadyStatus(int _playerRefId, bool _readyStatus) {
        ready[_playerRefId] = _readyStatus;
    }
}
