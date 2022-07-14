using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponShooter : MonoBehaviour
{
    abstract public void Shoot();

    abstract public void ReceiveShoot(string _projectileRefId, float _projectileDirectionRotation);
}
