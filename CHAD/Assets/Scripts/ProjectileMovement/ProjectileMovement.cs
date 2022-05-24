using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ProjectileMovement
{
    void SendMove();
    void Face();
    void DestroyProjectile();
    void ReceiveDestroyProjectile();
    void ReceiveMovement(Vector2 _position);
    void SetStats(float _speed, float _range, GameObject _origin, 
            Vector3 _originLocationVector, Vector3 _directionVector, float _rotationOffset);
}
