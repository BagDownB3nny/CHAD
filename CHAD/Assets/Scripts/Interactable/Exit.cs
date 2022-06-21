using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Exit : Interactable
{

    public override void OnInteract(GameObject player)
    {
        GameUIManager.instance.objectiveText.SetActive(false);
        MapManager.instance.LoadMap();
    }

    public override string GetText()
    {
        return "ENTER";
    }
}
