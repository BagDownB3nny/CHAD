using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Exit : Interactable
{
    private bool isOpen = false;
    public Sprite open;
    public Sprite closed;

    public void SetOpen() {
        isOpen = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = open;
    }

    public override void OnInteract(GameObject player)
    {
        if (isOpen) {
            MapManager.instance.LoadMap();
        }
        
    }

    public override string GetText()
    {
        if (isOpen) {
            return "PRESS " + InputManager.instance.keybinds[PlayerInputs.Interact] + " TO ENTER";
        } else {
            return "HOLE IS BLOCKED";
        }
    }
}
