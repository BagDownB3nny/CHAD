using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class JoinServerMenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string joinRoomScreen;
    public string backScreen;
    public TMP_InputField serverIp;

    public void LoadJoinRoom() {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        NetworkManager.SetGameType(GameType.Client);
        PlayerClient.instance.SetServerIp(serverIp.text);
        PlayerClient.instance.ConnectToServer();
    }

    public void Back() {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        SceneManager.LoadScene(backScreen);
    }
}
