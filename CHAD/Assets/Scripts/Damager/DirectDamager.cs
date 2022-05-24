using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DirectDamager : Damager
{
    float CalculateDamage(float _rawDamage, float _attack, float _armourPenetration,
            float _targetArmour, float _targetArmourEffectiveness);   
}
