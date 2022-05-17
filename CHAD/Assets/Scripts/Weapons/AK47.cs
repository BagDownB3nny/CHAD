using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47 : Gun
{
    [Header("AK47 Parameters")]
    public GameObject ammo;
    public float bulletRotationOffset = -90;

    void Update()
    {
        //player heading
        FiringDirection();
        //final bullet heading with inaccuracy
        BulletDirection();
        PointAtMouse();
        if (Input.GetMouseButton(0)){
            Debug.Log("fire");
            Fire();
        }
    }

    public override void Fire() {
        if (timeToNextShot <= 0) {
            GameObject bullet = Instantiate(ammo, transform.position, Quaternion.Euler(0f, 0f, directionRotation + bulletRotationOffset));
            bullet.GetComponent<PlayerProjectile>().SetParameters(transform.position, bulletSpeed, damage, range);
            bullet.GetComponent<Rigidbody2D>().velocity = (Vector2) bulletDirectionVector * bulletSpeed;
            timeToNextShot = shotInterval;
        } else {
            timeToNextShot -= Time.deltaTime;
        }
        
    }    
}
