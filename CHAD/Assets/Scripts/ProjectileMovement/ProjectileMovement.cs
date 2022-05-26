using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ProjectileMovement
{

    void SendMove();
    void DestroyProjectile();
    void ReceiveDestroyProjectile();
    void ReceiveMovement(Vector2 _position);
}
