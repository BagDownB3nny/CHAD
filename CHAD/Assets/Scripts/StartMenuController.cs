using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string singleplayerScreen;
    public string mulitplayerScreen;
    public string settingsScreen;

    public void LoadSingleplayer() {
        SceneManager.LoadScene(singleplayerScreen);
    }

    public void LoadMultiplayer() {
        SceneManager.LoadScene(mulitplayerScreen);
    }

    public void LoadSettings() {
        SceneManager.LoadScene(settingsScreen);
    }

    public void Exit() {
        Application.Quit();
    }
}
