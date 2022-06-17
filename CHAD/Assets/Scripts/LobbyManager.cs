using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    public Dictionary<string, bool> ready = new Dictionary<string, bool>();
    private bool myReadyStatus = false;
    public GameObject readyToggle;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }
    }

    private void Start()
    {
        if (NetworkManager.gameType == GameType.Server)
        {
            readyToggle.SetActive(false);
        }
    }

    public void SpawnIn()
    {
        ClientSend.MapLoaded();
        readyToggle.SetActive(true);
    }

    public void SpawnPlayer(string _playerId) {
        ready.Add(_playerId, false);
    }

    public void ToggleReadyStatus(bool toggle) {
        myReadyStatus = readyToggle.GetComponent<Toggle>().isOn;
        if (myReadyStatus)
        {
            readyToggle.transform.GetChild(1).GetComponent<Text>().color = Color.green;
        }
        else
        {
            readyToggle.transform.GetChild(1).GetComponent<Text>().color = Color.red;
        }
        SendReadyStatus();
    }

    public void ReadyStatus(string _playerRefId, bool _readyStatus) {
        ready[_playerRefId] = _readyStatus;
        foreach (KeyValuePair<string, bool> readyStatus in ready) {
            if (!readyStatus.Value) {
                ServerSend.ReadyStatus(_playerRefId, _readyStatus);
                return;
            } 
        }
        if (ready.Count > 0) {
            //TODO: countdown
            MapManager.instance.LoadMap();
        }
    }

    public void SendReadyStatus() {
        ClientSend.ReadyStatus(PlayerClient.instance.myId, myReadyStatus);
    }

    public void ReceiveReadyStatus(string _playerRefId, bool _readyStatus) {
        ready[_playerRefId] = _readyStatus;
        foreach (KeyValuePair<string, bool> ready in ready)
        {
            Debug.Log(ready.Key + " is " + ready.Value.ToString());
        }
    }
}
