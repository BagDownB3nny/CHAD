using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomUIManager : MonoBehaviour
{


    public void SpawnIn() {
        GameManager.instance.SpawnWaitingRoomPlayer();
        gameObject.SetActive(false);

    }
}
