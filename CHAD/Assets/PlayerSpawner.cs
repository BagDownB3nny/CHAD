using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public void Start()
    {
        GameManager.instance.playerSpawner = this;
    }

    public void SpawnPlayer(int _playerId, PlayerClasses _playerClass)
    {
        GameObject player = Instantiate(GameManager.instance.playerPrefabs[(int)_playerClass]);
        player.GetComponent<PlayerStatsManager>().characterRefId = _playerId.ToString();
        // If playerInfo exists
        if (PlayerInfoManager.AllPlayerInfo.ContainsKey(_playerId.ToString()))
        {
            PlayerInfo playerInfo = PlayerInfoManager.AllPlayerInfo[_playerId.ToString()];
            // If playerInfo is of the same playerClass, that means we are spawning in a saved player
            if (playerInfo.playerClass == _playerClass)
            {
                player.GetComponent<PlayerStatsManager>().SetStats(playerInfo);
                // If GameManager already contains a character, replace that character with new character
                if (GameManager.instance.players.ContainsKey(_playerId.ToString()))
                {
                    Vector2 playerPos = GameManager.instance.players[_playerId.ToString()].transform.position;
                    player.transform.position = playerPos;
                    Destroy(GameManager.instance.players[_playerId.ToString()]);
                    GameManager.instance.players[_playerId.ToString()] = player;
                } // If GameManager does not contain a character, add new character
                else
                {
                    GameManager.instance.players.Add(_playerId.ToString(), player);
                }
            } // If playerInfo is of different playerClass, that means we are trying to change playerClass of existing player
            else
            {
                playerInfo.ChangeClass(_playerClass, player.GetComponent<PlayerStatsManager>());
                try
                {
                    player.transform.position = GameManager.instance.players[_playerId.ToString()].transform.position;
                    Destroy(GameManager.instance.players[_playerId.ToString()]);
                    GameManager.instance.players[_playerId.ToString()] = player;
                } catch (Exception _ex)
                {
                    Debug.Log("Cannot change player class: Player does not exist in GameManager");
                }
            }
        } // If playerInfo does not exist, spawn in default player and create a new playerInfo
        else
        {
            PlayerInfoManager.Initialize(_playerId.ToString(), _playerClass, player.GetComponent<PlayerStatsManager>());
            GameManager.instance.players.Add(_playerId.ToString(), player);
        }
    }
}
