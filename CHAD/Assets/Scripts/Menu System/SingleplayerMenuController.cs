using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SingleplayerMenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string newGameScreen;
    public string loadGameScreen;
    public string backScreen;

    public void LoadNewGame() {
        SceneManager.LoadScene(newGameScreen);
    }

    public void LoadLoadGame() {
        SceneManager.LoadScene(loadGameScreen);
    }

    public void Back() {
        SceneManager.LoadScene(backScreen);
    }
}
