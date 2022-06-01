using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerRangedWeapon : RangedWeapon
{
    private void Start() {
        holder.GetComponent<CharacterStatsManager>().target
                = GameObject.Find("Crosshair");
    }

    void Update()
    {
        if (NetworkManager.IsMine(holder.GetComponent<CharacterStatsManager>().characterRefId)) {
            CalculateDirectionVector();
            PointToTarget();
            ClientSend.RotateRangedWeapon(holder.GetComponent<CharacterStatsManager>().characterRefId
                    , directionRotation);

            if (Input.GetMouseButton(0) && CanAttack()) {
                SendAttack();
            }
        }
        timeToNextAttack -= Time.deltaTime;
    }

    public void SendAttack() {
        Debug.Log("Sending attack");
        ClientSend.RangedAttack(holder.GetComponent<CharacterStatsManager>().characterRefId);
    }
}
