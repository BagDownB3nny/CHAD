using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrops : Drops
{
    public PlayerWeapons playerWeapon;
    public GameObject gun;

    private void Start()
    {
        gun = Instantiate(GameManager.instance.gunPrefabs[(int)playerWeapon]);
        gun.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().sprite = gun.GetComponent<SpriteRenderer>().sprite;
    }

    public override void PickUp(GameObject player)
    {
        player.GetComponent<PlayerWeaponsManager>().AddGun(playerWeapon);
        ServerSend.AddGun(player.GetComponent<PlayerStatsManager>().characterRefId, playerWeapon);
        ServerSend.Broadcast("Server Sending Add Gun");
    }
}
