using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

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

        PlayerClient.instance.udp.Connect(
            ((IPEndPoint)PlayerClient.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int playerIdReceived = _packet.ReadInt();
        Vector2 position = _packet.ReadVector2();
        int characterType = _packet.ReadInt();
        Debug.Log("ClientHandle packet: " + playerIdReceived + position + characterType);
        GameManager.instance.SpawnPlayer(playerIdReceived.ToString(), characterType, position, true);
        LobbyManager.instance.ReceiveSpawnPlayer(playerIdReceived);
    }

    public static void MovePlayer(Packet _packet)
    {
        int _affectedPlayerId = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();
        if (IsPresent(GameManager.instance.players, _affectedPlayerId.ToString())) {
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
        string spawnerRefId = _packet.ReadString();
        string enemyRefId = _packet.ReadString();
        int enemyId = _packet.ReadInt();
        Vector2 position = _packet.ReadVector2();
        if (IsPresent(GameManager.instance.spawners, spawnerRefId)) {
            GameManager.instance.spawners[spawnerRefId].GetComponent<EnemySpawner>().ReceiveSpawnEnemy(enemyRefId, enemyId, position);
        }
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
            if (IsPresent(GameManager.instance.players, affectedCharacterRefId)) {
                GameManager.instance.players[affectedCharacterRefId].GetComponent<PlayerWeaponsManager>().weaponScript
                        .ReceiveRotateRangedWeapon(directionRotation);
            }
        } else if (characterType == CharacterType.Enemy) {
            if (IsPresent(GameManager.instance.players, affectedCharacterRefId)) {
                GameManager.instance.enemies[affectedCharacterRefId].GetComponent<EnemyWeaponManager>().rangedWeaponScript
                        .ReceiveRotateRangedWeapon(directionRotation);
            }
        }
    }

    public static void ReadyStatus(Packet _packet) {
        int playerRefId = _packet.ReadInt();
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
}
