using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : Items
{
    private void Awake()
    {
        playerItem = PlayerItems.Armour;
    }

    public override void OnPickUp(string _playerRefId)
    {
        GameObject player = GameManager.instance.players[_playerRefId];

        player.GetComponent<PlayerStatsManager>().armour += 0.04f;
    }
}
