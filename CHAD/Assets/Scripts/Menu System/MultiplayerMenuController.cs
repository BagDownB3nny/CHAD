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
        NetworkManager.instance.SetGameType(GameType.Server);
        Server.Start(4, 26950);
    }

    public void LoadJoinRoom() {
        SceneManager.LoadScene(joinRoomScreen);
        NetworkManager.instance.SetGameType(GameType.Client);
        PlayerClient.instance.ConnectToServer();
    }

    public void Back() {
        SceneManager.LoadScene(backScreen);
    }
}
