using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMover : MonoBehaviour
{
    public void StartBossFight()
    {

    }

    public void Move(Vector3 direction)
    {
        transform.Translate(direction);
        ServerSend.MoveBoss(transform.position);
    }

    public void ReceiveMove(Vector3 _pos)
    {
        transform.position = _pos;
    }
}
