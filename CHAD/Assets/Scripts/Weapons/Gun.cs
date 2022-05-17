using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    protected Vector3 directionVector;
    protected Vector3 bulletDirectionVector;
    protected float directionRotation;
    protected float timeToNextShot;
    [Header("Gun Parameters")]
    public float damage;
    public float bulletSpeed;
    public float range;
    public float accuracy;
    public float shotInterval;
    public float gunRotationOffset = 0;


    //points this gameObject at the mouse
    public void PointAtMouse() {
        transform.rotation = Quaternion.Euler(0f, 0f, directionRotation + gunRotationOffset);
        if (directionRotation >= -90 && directionRotation < 89) {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        } else {
            gameObject.transform.localScale = new Vector3(1, -1, 1);
        }
    }

    //returns a normalized direction vector from obj to end
    public void FiringDirection() {
        directionVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        directionRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
    }

    public void BulletDirection() {
        float rand = Random.Range(-accuracy, accuracy);
        bulletDirectionVector = Quaternion.AngleAxis(rand, Vector3.back) * directionVector;
    }

    public abstract void Fire();

    public void Discard() {
        Destroy(gameObject);
    }
}
