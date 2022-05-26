using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerRangedWeapon : RangedWeapon
{

    void Update()
    {
        CalculateDirectionVector();
        PointToTarget();
        ClientSend.RotateRangedWeapon(directionRotation);
        if (Input.GetMouseButton(0) && NetworkManager.IsMine(
                    holder.GetComponent<CharacterStatsManager>().characterRefId)) {
            if (CanAttack()) {
                SendAttack();
            }
        }
    }
    public void ReceiveGunRotation(float _rotation)
    {
        transform.rotation = Quaternion.Euler(0, 0, _rotation);
        if (directionRotation >= -90 && directionRotation < 89)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1, -1, 1);
        }
    }

    public void SendAttack() {
        ClientSend.RangedAttack(holder.GetComponent<CharacterStatsManager>().characterRefId);
    }
}
