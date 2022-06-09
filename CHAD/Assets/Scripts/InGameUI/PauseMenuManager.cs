using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public void Disconnect() {
        if (NetworkManager.gameType == GameType.Client) {
            Debug.Log("Disconnecting");
            PlayerClient.instance.Disconnect();
        }
    }
}
