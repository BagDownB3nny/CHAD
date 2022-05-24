using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStatsManager : MonoBehaviour
{
    public float attack;
    public float armourPenetration;
    public abstract void TakeDamage(float _damageTaken, float _armourPenetration);
}
