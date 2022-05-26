using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DirectDamager : Damager
{
    public abstract float CalculateDamageDealt(float _rawDamage, float _attack);   
}
