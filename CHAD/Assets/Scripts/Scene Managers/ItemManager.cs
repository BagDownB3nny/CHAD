using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerItems
{
    Rat = 0,
    Monkey = 1,
    FlySwatter = 2,
    TF2Hat = 3
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public Dictionary<string, GameObject> itemDrops;
    public Dictionary<string, GameObject> weaponDrops;
    public GameObject itemDropPrefab;
    public GameObject weaponDropPrefab;

    public int[] itemDropRange = new int[] { 4, 6 };
    public int itemsToDrop;
    public int itemsDropped;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            EnemyDeath.onEnemyDeath += OnEnemyDeath;
            itemDrops = new Dictionary<string, GameObject>();
            weaponDrops = new Dictionary<string, GameObject>();
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartDropping()
    {
        if (NetworkManager.gameType == GameType.Server)
        {
            itemsToDrop = Mathf.RoundToInt(UnityEngine.Random.Range(itemDropRange[0], itemDropRange[1]));
            itemsDropped = 0;
        }
    }

    private void OnEnemyDeath(GameObject deadEnemy)
    {
        if (NetworkManager.gameType == GameType.Server && itemsDropped < itemsToDrop)
        {
            float probability = EnemySpawner.instance.enemiesLeftToSpawn;
            if (probability > UnityEngine.Random.Range(0, 1))
            {
                int dropType = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));
                // Drop weapon
                if (dropType == 0)
                {
                    GameObject weadponDrop = Instantiate(weaponDropPrefab,
                            deadEnemy.transform.position, Quaternion.identity);
                    PlayerWeapons droppedWeapon = (PlayerWeapons) Mathf.RoundToInt(
                            UnityEngine.Random.Range(0,
                            Enum.GetNames(typeof(PlayerWeapons)).Length));
                    string dropId = itemsDropped.ToString();
                    weadponDrop.GetComponent<WeaponDrops>().playerWeapon = droppedWeapon;
                    weadponDrop.GetComponent<WeaponDrops>().dropId = dropId;
                    weaponDrops.Add(dropId, weadponDrop);

                    ServerSend.WeaponDrop(dropId, droppedWeapon, deadEnemy.transform.position);
                }
                // Drop item
                if (dropType == 1)
                {
                    // TODO: Create items and make them droppable
                    GameObject drop = Instantiate(weaponDropPrefab,
                            deadEnemy.transform.position, Quaternion.identity);
                    PlayerWeapons droppedWeapon = (PlayerWeapons)Mathf.RoundToInt(
                            UnityEngine.Random.Range(0,
                            Enum.GetNames(typeof(PlayerWeapons)).Length));
                    drop.GetComponent<WeaponDrops>().playerWeapon = droppedWeapon;
                }
                itemsDropped += 1;
            }
        }
    }

    public void ReceiveWeaponDrop(string _dropId, PlayerWeapons _droppedWeapon, Vector2 _position)
    {
        GameObject drop = Instantiate(weaponDropPrefab, _position, Quaternion.identity);
        drop.GetComponent<WeaponDrops>().playerWeapon = _droppedWeapon;
        weaponDrops.Add(_dropId, drop);
    }

    public void ResetItems()
    {
        foreach (GameObject item in itemDrops.Values)
        {
            Destroy(item);
        }
        itemDrops.Clear();
        foreach (GameObject weapon in weaponDrops.Values)
        {
            Destroy(weapon);
        }
        weaponDrops.Clear();
    }

    public void RemoveWeaponDrop(string _dropId)
    {
        Destroy(weaponDrops[_dropId]);
        weaponDrops.Remove(_dropId);
    }

    public void ReceiveRemoveWeaponDrop(string _dropId)
    {
        RemoveWeaponDrop(_dropId);
    }
}
