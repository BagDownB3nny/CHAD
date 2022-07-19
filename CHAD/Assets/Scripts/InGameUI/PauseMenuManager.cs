using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    public GameObject settingsMenu;
    [SerializeField]
    public GameObject controlsMenu;
    [SerializeField]
    public GameObject audioMenu;

    public void Disconnect() {
        if (NetworkManager.gameType == GameType.Client) {
            SoundManager.instance.PlaySound(Sounds.ButtonPress);
            PlayerClient.instance.Disconnect();
            GameManager.instance.ResetToMainMenu();
        }
    }

    public void Settings()
    {
        if (NetworkManager.gameType == GameType.Client)
        {
            SoundManager.instance.PlaySound(Sounds.ButtonPress);
            settingsMenu.SetActive(true);
        }
    }

    public void Back()
    {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        controlsMenu.SetActive(true);
        audioMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void Controls()
    {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        controlsMenu.SetActive(true);
        audioMenu.SetActive(false);
    }

    public void Audio()
    {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        controlsMenu.SetActive(false);
        audioMenu.SetActive(true);
    }
}
