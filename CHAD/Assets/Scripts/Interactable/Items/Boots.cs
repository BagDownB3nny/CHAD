using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boots : Items
{
    private void Awake()
    {
        playerItem = PlayerItems.Boot;
    }

    public override void OnPickUp(string _playerRefId)
    {
        GameObject player = GameManager.instance.players[_playerRefId];

        Debug.Log("Obtained boot (client)");
    }
}
