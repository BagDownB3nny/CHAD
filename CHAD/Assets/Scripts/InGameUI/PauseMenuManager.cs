using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject settingsMenu;

    public void Disconnect() {
        if (NetworkManager.gameType == GameType.Client) {
            PlayerClient.instance.Disconnect();
            GameManager.instance.ResetToMainMenu();
        }
    }

    public void Settings()
    {
        if (NetworkManager.gameType == GameType.Client)
        {
            settingsMenu.SetActive(true);
        }
    }
}
