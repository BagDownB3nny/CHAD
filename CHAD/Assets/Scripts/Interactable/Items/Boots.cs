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

        player.GetComponent<PlayerStatsManager>().speed += 0.5f;
    }
}
