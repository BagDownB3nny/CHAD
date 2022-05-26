using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damager
{
    void DealDamage(GameObject _target, float _damageDealt);

    void DestroyDamager();
}
