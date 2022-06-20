using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Interactable
{

    public override void OnInteract(GameObject player)
    {
        MapManager.instance.LoadMap();
    }

    public override string GetText()
    {
        return "ENTER";
    }
}
