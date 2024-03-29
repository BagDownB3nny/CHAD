using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour, Death
{
    public GameObject deathEffect;

    public void Die() {
        if (deathEffect != null) {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        if (NetworkManager.IsMine(gameObject.GetComponent<PlayerStatsManager>().characterRefId)) {
            CameraMotor.instance.SetPlayerDeath(true);
        }
        Destroy(gameObject);
    }
}