using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterStatsManager
{
    void UpdateMovementStats();
    void UpdateAttackStats();
    void UpdateTargetStats(GameObject _damager);
    void TakeDamage(float _damageTaken);
}
