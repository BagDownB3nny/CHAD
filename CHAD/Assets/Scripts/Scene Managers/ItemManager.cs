using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public Dictionary<string, GameObject> itemDrops;
    public Dictionary<string, GameObject> weaponDrops;
    public GameObject itemDropPrefab;
    public GameObject weaponDropPrefab;

    private int[] itemDropRange = new int[] { 30, 31 };
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
            float probability = (float)(itemsToDrop - itemsDropped) /
                    (float)(EnemySpawner.instance.totalEnemiesToSpawn - EnemySpawner.instance.enemiesKilled);
            if (probability > UnityEngine.Random.Range(0, 1.0f))
            {
                int dropType = Mathf.RoundToInt(UnityEngine.Random.Range(0, 2));
                ServerSend.Broadcast(dropType.ToString());
                // Drop weapon
                if (dropType == 0)
                {
                    GameObject weaponDrop = Instantiate(weaponDropPrefab,
                            deadEnemy.transform.position, Quaternion.identity);
                    PlayerWeapons droppedWeapon = (PlayerWeapons) Mathf.RoundToInt(
                            UnityEngine.Random.Range(1,
                            Enum.GetNames(typeof(PlayerWeapons)).Length));
                    string dropId = itemsDropped.ToString();
                    weaponDrop.GetComponent<WeaponDrops>().playerWeapon = droppedWeapon;
                    weaponDrop.GetComponent<WeaponDrops>().dropId = dropId;
                    weaponDrops.Add(dropId, weaponDrop);

                    ServerSend.WeaponDrop(dropId, droppedWeapon, deadEnemy.transform.position);
                }
                // Drop item
                if (dropType == 1)
                {
                    GameObject itemDrop = Instantiate(itemDropPrefab,
                            deadEnemy.transform.position, Quaternion.identity);
                    //PlayerItems droppedItem = (PlayerItems)Mathf.RoundToInt(
                    //        UnityEngine.Random.Range(1,
                    //        Enum.GetNames(typeof(PlayerItems)).Length));
                    PlayerItems droppedItem = PlayerItems.Boot;
                    string dropId = itemsDropped.ToString();
                    itemDrop.GetComponent<ItemDrops>().SetItemType(droppedItem);
                    itemDrop.GetComponent<ItemDrops>().dropId = dropId;
                    itemDrops.Add(dropId, itemDrop);
                    ServerSend.ItemDrop(dropId, droppedItem, deadEnemy.transform.position);
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

    public void ReceiveItemDrop(string _dropId, PlayerItems _droppedItem, Vector3 _position)
    {
        GameObject drop = Instantiate(itemDropPrefab, _position, Quaternion.identity);
        drop.GetComponent<ItemDrops>().SetItemType(_droppedItem);
        drop.GetComponent<ItemDrops>().dropId = _dropId;
        itemDrops.Add(_dropId, drop);
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

    public void RemoveItemDrop(string _dropId)
    {
        Destroy(itemDrops[_dropId]);
        weaponDrops.Remove(_dropId);
    }

    public void ReceiveRemoveItemDrop(string _dropId)
    {
        RemoveItemDrop(_dropId);
    }
}
