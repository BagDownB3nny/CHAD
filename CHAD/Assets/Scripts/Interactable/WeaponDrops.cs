using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrops : Interactable
{
    public PlayerWeapons playerWeapon;
    public GameObject gun;
    public string dropId;

    // Animation parameters
    Vector2 position;
    float x = 0;
    float speed = 0.1f;
    float amplitude = 0.2f;

    private void Start()
    {
        gun = Instantiate(GameManager.instance.gunPrefabs[(int)playerWeapon]);
        gun.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().sprite = gun.GetComponent<SpriteRenderer>().sprite;
        position = transform.position;
    }

    private void FixedUpdate()
    {
        Animate();
    }

    private void Animate()
    {
        x += speed;
        transform.position = new Vector2(position.x, position.y + amplitude * Mathf.Sin(x));
    }

    public override void OnInteract(GameObject player)
    {
        player.GetComponent<PlayerWeaponsManager>().AddGun(playerWeapon);
        ServerSend.AddGun(player.GetComponent<PlayerStatsManager>().characterRefId, playerWeapon);
        // TODO: Implement a system to create drop ids for items dropped
        // ServerSend.DestroyGunDrop(dropId);
    }

    public override string GetText()
    {
        return "PICK UP";
    }
}
