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
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        SceneManager.LoadScene(hostRoomScreen);
        NetworkManager.SetGameType(GameType.Server);
        Server.Start(4, 26950);
        MusicManager.instance.PlayMusic(Music.none);
    }

    public void LoadJoinRoom() {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        SceneManager.LoadScene(joinRoomScreen);
    }

    public void Back() {
        SoundManager.instance.PlaySound(Sounds.ButtonPress);
        SceneManager.LoadScene(backScreen);
    }
}
