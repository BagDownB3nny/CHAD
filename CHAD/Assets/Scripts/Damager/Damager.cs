using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damager
{
    void DealDamage(GameObject _target, float _finalDamage);

    void DestroyDamager();

    void SetStats(GameObject _holder, float _attack, float _armourPenetration, float _damage, string _targetType);

    void SetAttackerStats(float _attack, float _armourPenetration);

    void SetTargetStats(float _targetArmour, float _targetArmourEffectiveness);
}
