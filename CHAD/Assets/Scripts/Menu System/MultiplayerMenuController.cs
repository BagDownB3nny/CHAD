using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiplayerMenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string hostRoomScreen;
    public string joinRoomScreen;
    public string backScreen;

    public void LoadHostRoom() {
        SceneManager.LoadScene(hostRoomScreen);
        NetworkManager.SetGameType(GameType.Server);
        Server.Start(4, 26950);
        MusicManager.instance.PlayMusic(Music.none);
    }

    public void LoadJoinRoom() {
        SceneManager.LoadScene(joinRoomScreen);
    }

    public void Back() {
        SceneManager.LoadScene(backScreen);
    }
}
