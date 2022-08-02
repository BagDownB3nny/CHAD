using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSword : Items
{
    private void Awake()
    {
        playerItem = PlayerItems.BFSword;
    }

    public override void OnPickUp(string _playerRefId)
    {
        GameObject player = GameManager.instance.players[_playerRefId];

        player.GetComponent<PlayerStatsManager>().attack += 0.04f;
    }
}
