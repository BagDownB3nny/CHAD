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
            if (Input.GetKeyDown(InputManager.instance.keybinds[PlayerInputs.Interact])) {
                ClientSend.Interact();
            }
        }
    }

    public void AddItem(PlayerItems playerItem)
    {
        SoundManager.instance.PlaySound(Sounds.Interact);
        GameObject item = GameManager.instance.itemDrops[(int) playerItem];

        // Gets item to perform its pickup function
        item.GetComponent<Items>().OnPickUp(GetComponent<PlayerStatsManager>().characterRefId);

        // Adds item to inventory
        PlayerItems newItem = item.GetComponent<Items>().playerItem;
        if (items.ContainsKey(newItem)) {
            items[newItem] += 1;
        } else
        {
            items.Add(newItem, 1);
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
