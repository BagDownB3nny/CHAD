using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    //the GameObject that produced this Projectile
    public GameObject origin;
    public Vector3 startLocation;

    [Header("Ammo Parameters")]
    public float speed;
    public float damage;
    public float range;

    public void SetParameters(Vector3 startLocation, float speed, float damage, float range) {
        this.startLocation = startLocation;
        this.speed = speed;
        this.damage = damage;
        this.range = range;
    }
}
