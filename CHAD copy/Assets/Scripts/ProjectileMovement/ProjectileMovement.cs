using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ProjectileMovement
{
    void Move();
    void Face();
    void DestroyProjectile();
    void SetStats(float _speed, float _range, GameObject _origin, 
            Vector3 _originLocationVector, Vector3 _directionVector, float _rotationOffset);
}
