using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerRangedWeapon : RangedWeapon
{
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
}
