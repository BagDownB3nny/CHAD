using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElvenAccuracy : Items
{
    private void Awake()
    {
        playerItem = PlayerItems.ElvenAccuracy;
    }

    public override void OnPickUp(string _playerRefId)
    {
        GameObject player = GameManager.instance.players[_playerRefId];

        player.GetComponent<PlayerStatsManager>().accuracy += 2f;
    }
}
