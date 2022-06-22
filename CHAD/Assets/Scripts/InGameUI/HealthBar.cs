using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        PlayerSpawner.onPlayerSpawn += OnPlayerSpawn;
    }

    public void OnPlayerSpawn(int _playerId)
    {
        if (NetworkManager.IsMine(_playerId.ToString()))
        {
            GameObject player = GameManager.instance.players[_playerId.ToString()];
            player.GetComponent<PlayerStatsManager>().healthBar = this;
            slider.maxValue = player.GetComponent<PlayerStatsManager>().maxHp;
            slider.value = player.GetComponent<PlayerStatsManager>().maxHp;
        }
    }
    
    public void SetHealth(float _hp) {
        slider.value = _hp;
    }


}
