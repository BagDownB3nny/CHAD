using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrops : Interactable
{
    public PlayerWeapons playerWeapon;
    public GameObject gun;
    public string dropId;

    private void Start()
    {
        gun = Instantiate(GameManager.instance.gunPrefabs[(int)playerWeapon]);
        gun.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().sprite = gun.GetComponent<SpriteRenderer>().sprite;
    }

    public override void OnInteract(GameObject player)
    {
        player.GetComponent<PlayerWeaponsManager>().AddGun(playerWeapon);
        ServerSend.AddGun(player.GetComponent<PlayerStatsManager>().characterRefId, playerWeapon);
        // TODO: Implement a system to create drop ids for items dropped
        // ServerSend.DestroyGunDrop(dropId);
    }
}
