using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerRangedWeapon : RangedWeapon
{

    public PlayerWeapons myWeapon;
    public int myId;

    //points this gameObject at the mouse
    public void PointAtMouse() {
        transform.rotation = Quaternion.Euler(0f, 0f, directionRotation + gunRotationOffset);
        if (directionRotation >= -90 && directionRotation < 89)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1, -1, 1);
        }
        ClientSend.SendGunRotation(transform.rotation);
    }

    //returns a normalized direction vector from obj to end
    public void FiringDirection() {
        //if (NetworkManager.instance.gameType == GameType.Client)
        //{
        //    directionVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        //    ClientSend.PlayerRotation(directionVector);
        //}
        directionVector = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        directionRotation = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
    }

    public void ReceiveGunRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
        if (directionRotation >= -90 && directionRotation < 89)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1, -1, 1);
        }
    }

    public abstract void SendAttack();
    public abstract object[] Attack(PlayerWeapons _gunType, float _directionRotation);
    public abstract GameObject ReceiveAttack(PlayerWeapons _gunType, float _bulletDirectionRotation);

    

    //public void SetFiringDirection(Vector3 _direction)
    //{
    //    if (NetworkManager.instance.gameType == GameType.Server)
    //    {
    //        directionVector = _direction;
    //    }
    //}
}
