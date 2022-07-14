using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPointer;

    public delegate void OnPlayerSpawn(int _playerId);
    public static OnPlayerSpawn onPlayerSpawn;

    public static PlayerSpawner instance;

    public int playersSpawned;

    public void Awake()
    {
        instance = this;
        playersSpawned = 0;
    }

    public void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public void SpawnPlayer(int _playerId, PlayerClasses _playerClass)
    {
        // If playerInfo exists
        if (PlayerInfoManager.AllPlayerInfo.ContainsKey(_playerId.ToString()))
        {
            PlayerInfo playerInfo = PlayerInfoManager.AllPlayerInfo[_playerId.ToString()];
            // If playerInfo is of the same playerClass, that means we are spawning in a saved player
            if (playerInfo.playerClass == _playerClass)
            {
                // If GameManager already contains a character, replace that character with new character (only happens in lobby)
                if (GameManager.instance.players.ContainsKey(_playerId.ToString()))
                {
                    return;
                } // If GameManager does not contain a character, add new character
                else
                {
                    GameObject player = Instantiate(GameManager.instance.playerPrefabs[(int)_playerClass], transform.position, Quaternion.identity);
                    player.GetComponent<PlayerStatsManager>().characterRefId = _playerId.ToString();
                    player.GetComponent<PlayerStatsManager>().SetStats(playerInfo);
                    player.GetComponent<PlayerWeaponsManager>().SetWeaponInventory(playerInfo);

                    GameManager.instance.players.Add(_playerId.ToString(), player);

                    //spawn the player pointer too
                    if (NetworkManager.gameType == GameType.Client && !NetworkManager.IsMine(_playerId.ToString())) {
                        GameUIManager.instance.InstantiaitePlayerPointer(player, playerPointer, _playerId);
                    }
                }
            } // If playerInfo is of different playerClass, that means we are trying to change playerClass of existing player
            else
            {
                GameObject player = Instantiate(GameManager.instance.playerPrefabs[(int)_playerClass], transform.position, Quaternion.identity);
                player.GetComponent<PlayerStatsManager>().characterRefId = _playerId.ToString();
                playerInfo.ChangeClass(_playerClass, player.GetComponent<PlayerStatsManager>());
                if (NetworkManager.IsMine(_playerId.ToString()))
                {
                    LobbyManager.instance.readyToggle.GetComponent<Toggle>().isOn = false;
                }
                try
                {


                    player.transform.position = GameManager.instance.players[_playerId.ToString()].transform.position;
                    Destroy(GameManager.instance.players[_playerId.ToString()]);
                    if (NetworkManager.IsMine(_playerId.ToString()))
                    {
                        GameUIManager.instance.weaponWheel.GetComponent<WeaponWheel>().ResetWheel();
                    }
                    player.GetComponent<PlayerWeaponsManager>().AddGun(PlayerWeapons.TestRifle);
                    GameManager.instance.players[_playerId.ToString()] = player;
                } catch (Exception _ex)
                {
                    Debug.Log("Cannot change player class: Player does not exist in GameManager\n" + _ex);
                }
            }
        } // If playerInfo does not exist, spawn in default player and create a new playerInfo
        else
        {
            GameObject player = Instantiate(GameManager.instance.playerPrefabs[(int)_playerClass], transform.position, Quaternion.identity);
            player.GetComponent<PlayerStatsManager>().characterRefId = _playerId.ToString();
            PlayerInfoManager.Initialize(_playerId.ToString(), _playerClass, player.GetComponent<PlayerStatsManager>());
            player.GetComponent<PlayerWeaponsManager>().AddGun(PlayerWeapons.TestRifle);

            GameManager.instance.players.Add(_playerId.ToString(), player);

            //spawn the player pointer too
            if (NetworkManager.gameType == GameType.Client && !NetworkManager.IsMine(_playerId.ToString())) {
                GameUIManager.instance.InstantiaitePlayerPointer(player, playerPointer, _playerId);
            }
        }
        if (onPlayerSpawn != null)
        {
            onPlayerSpawn(_playerId);
        }
        playersSpawned += 1;
        if (NetworkManager.gameType == GameType.Server && playersSpawned == Server.NumberOfPlayers)
        {
            if (GameManager.instance.IsBossLevel()) {
                EnemySpawner.instance.SpawnBoss();
                BossManager.instance.StartBossFight();
            }
            else if (EnemySpawner.instance != null)
            {
                EnemySpawner.instance.StartSpawning();
                ItemManager.instance.StartDropping();
            }
        }

    }
}
