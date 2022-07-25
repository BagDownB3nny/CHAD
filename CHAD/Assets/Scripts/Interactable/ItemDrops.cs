using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrops : Interactable
{
    public PlayerItems playerItem;
    public GameObject item;
    public string dropId;

    // Animation parameters
    Vector2 position;
    float x = 0;
    float speed = 0.1f;
    float amplitude = 0.2f;

    private void Start()
    {
        position = transform.position;
    }

    public void SetItemType(PlayerItems _playerItem)
    {
        playerItem = _playerItem;
        item = Instantiate(GameManager.instance.itemDrops[(int)playerItem]);
        item.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().sprite = item.GetComponent<SpriteRenderer>().sprite;
    }

    private void FixedUpdate()
    {
        Animate();
    }

    private void Animate()
    {
        x += speed;
        transform.position = new Vector2(position.x, position.y + amplitude * Mathf.Sin(x));
    }

    public override void OnInteract(GameObject player)
    {
        player.GetComponent<PlayerItemsManager>().AddItem(playerItem);
        ServerSend.AddItem(player.GetComponent<PlayerStatsManager>().characterRefId, playerItem);
        ItemManager.instance.RemoveItemDrop(dropId);
        ServerSend.RemoveItemDrop(dropId);
    }

    public override string GetText()
    {
        string extraText = "";
        if (playerItem == PlayerItems.BFSword) {
            extraText = "BFSword: Attack +0.04";
        } else if (playerItem == PlayerItems.Boot) {
            extraText = "Boots: Speed +0.5";
        } else if (playerItem == PlayerItems.Armour) {
            extraText = "Armour: Armour +0.04";
        } else if (playerItem == PlayerItems.ElvenAccuracy) {
            extraText = "Elven Accuracy: Accuracy +2";
        } else if (playerItem == PlayerItems.PiercingRounds) {
            extraText = "Piercing: Armour Penetration +0.5";
        }
        return extraText + "\n" + "PRESS " + InputManager.instance.keybinds[PlayerInputs.Interact] + " TO PICK UP";
    }
}
