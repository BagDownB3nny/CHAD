using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientHandle : MonoBehaviour
{

    public static bool IsPresent(Dictionary<string, GameObject> dict, string refId) {

        return dict.ContainsKey(refId) && dict[refId] != null;
    }
    public static void Welcome(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _msg = _packet.ReadString();

        Debug.Log($"Message from server: {_msg}");
        PlayerClient.instance.myId = _id;
        Debug.Log("I am player " + _id);

        PlayerClient.instance.udp.Connect(
            ((IPEndPoint)PlayerClient.instance.tcp.socket.Client.LocalEndPoint).Port);
        ClientSend.WelcomeReceived();
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int playerIdReceived = _packet.ReadInt();
        int characterType = _packet.ReadInt();
        PlayerSpawner.instance.SpawnPlayer(playerIdReceived, (PlayerClasses)characterType);
    }

    public static void MovePlayer(Packet _packet)
    {
        int _affectedPlayerId = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();
        if (GameManager.instance != null &&
                IsPresent(GameManager.instance.players, _affectedPlayerId.ToString())) {
            GameManager.instance.players[_affectedPlayerId.ToString()].GetComponent<PlayerMovement>().ReceiveMovement(_position);
        }
    }

    public static void RangedAttack(Packet _packet) {
        CharacterType characterType = (CharacterType) _packet.ReadInt();
        string affectedCharacterRefId = _packet.ReadString();
        string projectileRefId = _packet.ReadString();
        float projectileDirectionRotation = _packet.ReadFloat();
        if (characterType == CharacterType.Player) {
            if (IsPresent(GameManager.instance.players, affectedCharacterRefId)) {
                PlayerRangedWeapon weaponScript = GameManager.instance.players[affectedCharacterRefId].GetComponent<PlayerWeaponsManager>().weaponScript;
                if (weaponScript != null) {
                    weaponScript.ReceiveAttack(projectileRefId, projectileDirectionRotation);
                }
            }
        } else if (characterType == CharacterType.Enemy) {
            if (IsPresent(GameManager.instance.enemies, affectedCharacterRefId)) {
                RangedWeapon weaponScript = GameManager.instance.enemies[affectedCharacterRefId].GetComponent<EnemyWeaponManager>().rangedWeaponScript;
                if (weaponScript != null) {
                    weaponScript.ReceiveAttack(projectileRefId, projectileDirectionRotation);
                }
            }
        } else if (characterType == CharacterType.Boss)
        {
            BossManager.instance.bossAttacker.ReceiveAttack(affectedCharacterRefId,
                    projectileRefId, projectileDirectionRotation);
        }
    }

    public static void MeleeAttack(Packet _packet) {
        CharacterType characterType = (CharacterType) _packet.ReadInt();
        string affectedCharacterRefId = _packet.ReadString();
        string damageDealerRefId = _packet.ReadString();
        if (IsPresent(GameManager.instance.enemies, affectedCharacterRefId) && GameManager.instance.enemies[affectedCharacterRefId]) {
            MeleeWeapon weaponScript = GameManager.instance.enemies[affectedCharacterRefId].GetComponent<EnemyWeaponManager>().meleeWeaponScript;
            if (weaponScript != null) {
                weaponScript.ReceiveAttack(damageDealerRefId);
            }
        }
    }

    public static void SpawnEnemy(Packet _packet) {
        string _enemyId = _packet.ReadString();
        int _enemyType = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();
        EnemySpawner.instance.ReceiveSpawnEnemy(_enemyId, _enemyType, _position);
    }

    public static void MoveProjectile(Packet _packet) {
        string _projectileRefId = _packet.ReadString();
        Vector2 _position  = _packet.ReadVector2();
        if (IsPresent(GameManager.instance.projectiles, _projectileRefId)) {
            GameManager.instance.projectiles[_projectileRefId].GetComponent<ProjectileMovement>()
                    .ReceiveMovement(_position);
        }
    }

    public static void DestroyProjectile(Packet _packet) {
        string _projectileRefId = _packet.ReadString();
        if (IsPresent(GameManager.instance.projectiles, _projectileRefId)) {
            GameManager.instance.projectiles[_projectileRefId].GetComponent<ProjectileMovement>()
                    .ReceiveDestroyProjectile();
        }
    }

    public static void DestroyDamageDealer(Packet _packet) {
        string damageDealerRefId = _packet.ReadString();
        if (IsPresent(GameManager.instance.damageDealers, damageDealerRefId)) {
            GameManager.instance.damageDealers[damageDealerRefId].GetComponent<Damager>().ReceiveDestroyDamager();
        }   
    }
    
    public static void MoveEnemy(Packet _packet) {
        string enemyRefId = _packet.ReadString();
        Vector2 position = _packet.ReadVector2();
        if (IsPresent(GameManager.instance.enemies, enemyRefId)) {
            GameManager.instance.enemies[enemyRefId].GetComponent<EnemyMovement>().ReceiveMove(position);
        }   
    }

    public static void TakeDamage(Packet _packet) {
        int characterType = _packet.ReadInt();
        string characterRefId = _packet.ReadString();
        float damageTaken = _packet.ReadFloat();
        if (characterType == (int) CharacterType.Player) {
            if (IsPresent(GameManager.instance.players, characterRefId)) {
                GameManager.instance.players[characterRefId].GetComponent<PlayerStatsManager>().ReceiveTakeDamage(damageTaken);
            }
        } else if (characterType == (int) CharacterType.Enemy) {
            if (IsPresent(GameManager.instance.enemies, characterRefId)) {
                GameManager.instance.enemies[characterRefId].GetComponent<EnemyStatsManager>().ReceiveTakeDamage(damageTaken);
            }
        }
    }

    public static void Die(Packet _packet) {
        int characterType = _packet.ReadInt();
        string characterRefId = _packet.ReadString();
        if (characterType == (int) CharacterType.Player) {
            if (IsPresent(GameManager.instance.players, characterRefId)) {
                GameManager.instance.players[characterRefId].GetComponent<PlayerStatsManager>().ReceiveDie();
            }
        } else if (characterType == (int) CharacterType.Enemy) {
            if (IsPresent(GameManager.instance.enemies, characterRefId)) {
                GameManager.instance.enemies[characterRefId].GetComponent<EnemyStatsManager>().ReceiveDie();
            }
        }
    }

    public static void RemovePlayer(Packet _packet) {
        string characterRefId = _packet.ReadString();
        Destroy(GameManager.instance.players[characterRefId]);
        GameManager.instance.players.Remove(characterRefId);
    }

    public static void RotateRangedWeapon(Packet _packet) {
        CharacterType characterType = (CharacterType) _packet.ReadInt();
        string affectedCharacterRefId = _packet.ReadString();
        float directionRotation = _packet.ReadFloat();
        if (characterType == CharacterType.Player) {
            if (GameManager.instance != null &&
                    IsPresent(GameManager.instance.players, affectedCharacterRefId) &&
                    GameManager.instance.players[affectedCharacterRefId].GetComponent<PlayerWeaponsManager>().weaponScript != null) {
                GameManager.instance.players[affectedCharacterRefId].GetComponent<PlayerWeaponsManager>().weaponScript
                        .ReceiveRotateRangedWeapon(directionRotation);
            }
        } else if (characterType == CharacterType.Enemy) {
            if (IsPresent(GameManager.instance.enemies, affectedCharacterRefId)) {
                GameManager.instance.enemies[affectedCharacterRefId].GetComponent<EnemyWeaponManager>().rangedWeaponScript
                        .ReceiveRotateRangedWeapon(directionRotation);
            }
        }
    }

    public static void ReadyStatus(Packet _packet) {
        string playerRefId = _packet.ReadString();
        bool readyStatus = _packet.ReadBool();
        LobbyManager.instance.ReceiveReadyStatus(playerRefId, readyStatus);
    }

    public static void ChangeClass(Packet _packet) {
        int playerRefId = _packet.ReadInt();
        int playerClass = _packet.ReadInt();
        Vector2 playerPosition = _packet.ReadVector2();
        GameManager.instance.ReceiveChangeClass(playerRefId, playerClass, playerPosition);
    }

    public static void Broadcast(Packet _packet) {
        string _msg = _packet.ReadString();
        GameManager.instance.Broadcast(_msg);
    }

     public static void EquipGun(Packet _packet) {
        int _gunIndex = _packet.ReadInt();
        int _playerRefId = _packet.ReadInt();
        GameManager.instance.players[_playerRefId.ToString()].GetComponent<PlayerWeaponsManager>().ReceiveEquipGun(_gunIndex);
    }

    public static void LoadLobby(Packet _packet)
    {
        SceneManager.LoadScene("WaitingRoom");
        MusicManager.instance.PlayMusic(Music.lobby);
        ClientSend.LobbyLoaded();
    }

    public static void LoadEmptyMap(Packet _packet)
    {
        MusicManager.instance.PlayMusic(Music.game);
        MapManager.instance.ReceiveLoadEmptyMap();
        
    }

    public static void LoadMap(Packet _packet)
    {
        MapType mapType = (MapType)_packet.ReadInt();
        string seed = _packet.ReadString();
        MapManager.instance.ReceiveLoadMap(mapType, seed);
    }

    public static void AddGun(Packet _packet)
    {
        string _affectedPlayer = _packet.ReadString();
        PlayerWeapons _gunType = (PlayerWeapons) _packet.ReadInt();
        GameManager.instance.players[_affectedPlayer].GetComponent<PlayerWeaponsManager>().AddGun(_gunType);
    }

    public static void UpdateEnemySpawnerStats(Packet _packet) {
        int _totalEnemiesToSpawn = _packet.ReadInt();
        int _enemiesLeftToSpawn = _packet.ReadInt();
        int _enemiesAlive = _packet.ReadInt();
        int _enemiesKilled = _packet.ReadInt();

        EnemySpawner.instance.ReceiveUpdateEnemySpawnerStats(_totalEnemiesToSpawn, _enemiesLeftToSpawn, _enemiesAlive, _enemiesKilled);
    }
    
    public static void WeaponDrop(Packet _packet)
    {
        string dropId = _packet.ReadString();
        PlayerWeapons droppedWeapon = (PlayerWeapons) _packet.ReadInt();
        Vector2 position = _packet.ReadVector2();
        ItemManager.instance.ReceiveWeaponDrop(dropId, droppedWeapon, position);
    }

    public static void RemoveWeaponDrop(Packet _packet)
    {
        string dropId = _packet.ReadString();
        ItemManager.instance.ReceiveRemoveWeaponDrop(dropId);
    }

    public static void AddItem(Packet _packet)
    {
        string _playerRefId = _packet.ReadString();
        PlayerItems _item = (PlayerItems)_packet.ReadInt();

        GameObject player = GameManager.instance.players[_playerRefId];
        player.GetComponent<PlayerItemsManager>().AddItem(_item);
    }

    public static void ItemDrop(Packet _packet)
    {
        string _dropId = _packet.ReadString();
        PlayerItems _droppedItem = (PlayerItems) _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        ItemManager.instance.ReceiveItemDrop(_dropId, _droppedItem, _position);
    }

    public static void RemoveItemDrop(Packet _packet)
    {
        string _dropId = _packet.ReadString();
        ItemManager.instance.ReceiveRemoveItemDrop(_dropId);
    }

    public static void SetBossAttack(Packet _packet)
    {
        string attackType = _packet.ReadString();
        int _bossAttack = _packet.ReadInt();
        BossManager.instance.bossAttacker.ReceiveSetBossAttack(attackType, _bossAttack);
    }

    public static void MoveBossAttack(Packet _packet)
    {
        BossWeaponType _attack = (BossWeaponType)_packet.ReadInt();
        Vector3 _pos = _packet.ReadVector3();
        BossManager.instance.bossAttacker.ReceiveMoveAttack(_attack, _pos);
    }

    public static void MoveBoss(Packet _packet)
    {
        Vector3 _pos = _packet.ReadVector3();
        BossManager.instance.bossMover.ReceiveMove(_pos);
    }
}
