using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DirectDamager : Damager
{
    float CalculateDamageDealt(float _rawDamage, float _attack);   
}
