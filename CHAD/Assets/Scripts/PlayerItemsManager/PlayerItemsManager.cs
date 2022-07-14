using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemsManager : MonoBehaviour
{
    public Dictionary<PlayerItems, int> items = new Dictionary<PlayerItems, int>();

    public delegate void Interact(GameObject player);
    public Interact interact;

    private void Update()
    {
        if (NetworkManager.IsMine(gameObject.GetComponent<PlayerStatsManager>().characterRefId)) {
            if (Input.GetKeyDown(KeyCode.E)) {
                ClientSend.Interact();
            }
        }
    }

    private void OnDestroy()
    {
        PlayerInfoManager.AllPlayerInfo[GetComponent<PlayerStatsManager>().characterRefId].SetItemInventory(this);
    }

    public void SetItemInventory(PlayerInfo playerInfo)
    {
        items = playerInfo.itemInventory;
    }
}
