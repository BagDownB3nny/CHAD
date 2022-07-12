using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeath : MonoBehaviour, Death
{
    public delegate void OnBossDeath(GameObject boss);
    public static OnBossDeath onBossDeath;
    public GameObject deathEffect;

    public void Die()
    {
        if (onBossDeath != null)
        {
            onBossDeath(gameObject);
        }
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}