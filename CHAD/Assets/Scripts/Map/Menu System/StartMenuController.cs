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
    public string mainScene;
    public GameObject canvas;

    public void LoadSingleplayer() {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        SceneManager.LoadScene(singleplayerScreen);
    }

    public void LoadMultiplayer() {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        SceneManager.LoadScene(mulitplayerScreen);
    }

    public void LoadSettings() {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        SceneManager.LoadScene(settingsScreen);
    }

    public void Exit() {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        Application.Quit();
    }
}
