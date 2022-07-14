using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttackMover : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    // Update is called once per frame
    public void Update()
    {
        if (NetworkManager.gameType == GameType.Server)
        {
            Move();
            ServerSend.MoveBossAttack(GetComponent<BossRangedWeapon>().bossWeaponType,
                    transform.position);
        }
    }

    public abstract void Move();

    public void ReceiveMove(Vector2 _pos)
    {
        transform.position = _pos;
    }
}
