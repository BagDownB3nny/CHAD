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
    public Dictionary<string, PlayerItems> itemDrops;
    public Dictionary<string, PlayerWeapons> weaponDrops;
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
                    GameObject drop = Instantiate(weaponDropPrefab,
                            deadEnemy.transform.position, Quaternion.identity);
                    PlayerWeapons droppedWeapon = (PlayerWeapons) Mathf.RoundToInt(
                            UnityEngine.Random.Range(0,
                            Enum.GetNames(typeof(PlayerWeapons)).Length));
                    drop.GetComponent<WeaponDrops>().playerWeapon = droppedWeapon;
                }
                // Drop item
                if (dropType == 1)
                {
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
}
