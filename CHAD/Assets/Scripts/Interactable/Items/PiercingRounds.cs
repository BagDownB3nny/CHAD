using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingRounds : Items
{
    private void Awake()
    {
        playerItem = PlayerItems.PiercingRounds;
    }

    public override void OnPickUp(string _playerRefId)
    {
        GameObject player = GameManager.instance.players[_playerRefId];

        player.GetComponent<PlayerStatsManager>().armourPenetration += 0.04f;
    }
}
