using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaitingRoomUIManager : MonoBehaviour
{

    public GameObject spawnIn;
    public GameObject ready;

    void Start()
    {
        if (NetworkManager.gameType == GameType.Server) {
            spawnIn.SetActive(false);
            ready.SetActive(false);
        }
    }    
    public void SpawnIn() {
        //GameManager.instance.playerSpawner.SpawnPlayer(PlayerClient.instance.myId, PlayerClasses.Captain);
        ClientSend.MapLoaded();
        spawnIn.SetActive(false);
        ready.SetActive(true);
    }

    public void toggleReady(bool toggle) {
        if (toggle) {
            ready.transform.GetChild(1).GetComponent<Text>().color = Color.green;
        } else {
            ready.transform.GetChild(1).GetComponent<Text>().color = Color.red;
        }
        ClientSend.ReadyStatus(PlayerClient.instance.myId, toggle);
    }
}
