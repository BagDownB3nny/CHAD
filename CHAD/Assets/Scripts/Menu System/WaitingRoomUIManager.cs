using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaitingRoomUIManager : MonoBehaviour
{

    public GameObject button;
    public void SpawnIn() {
        GameManager.instance.SpawnWaitingRoomPlayer();
        button.SetActive(false);

    }
}
