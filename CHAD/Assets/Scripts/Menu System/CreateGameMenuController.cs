using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateGameMenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string characterSelectScreen;
    public string GameScreen;
    public string backScreen;

    public void LoadGame() {
        SceneManager.LoadScene(GameScreen);
    }

    public void LoadCharacterSelect() {
        SceneManager.LoadScene(characterSelectScreen);
    }

    public void Back() {
        SceneManager.LoadScene(backScreen);
    }

    public void Easy() {
        //set easy
    }

    public void Medium() {
        //set med
    }

    public void Hard() {
        //set hard
    }
}
