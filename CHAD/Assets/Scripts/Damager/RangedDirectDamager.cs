using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDirectDamager : MonoBehaviour, DirectDamager
{
    [Header("Damager Parameters")]
    public float damage;
    string targetType;
    [Header("Attacker Parameters")]
    public GameObject holder;
    public float attack;
    public float armourPenetration;
    [Header("Target Parameters")]
    public float targetArmour;
    public float targetArmourEffectiveness;

    private void OnTriggerEnter2D(Collider2D _collider) {
        if (_collider.CompareTag(targetType)) {
            //copy over target parameters
            _collider.GetComponent<CharacterStatsManager>().UpdateTargetStats(gameObject);

            float finalDamage = CalculateDamage(damage, attack, armourPenetration, targetArmour, targetArmourEffectiveness);
            DealDamage(_collider.gameObject, finalDamage);
            DestroyDamager();
        }
    }

    public float CalculateDamage(float _rawDamage, float _attack, float _armourPenetration,
            float _targetArmour, float _targetArmourEffectiveness) {
                return (_rawDamage * _attack) * (1 - ((_targetArmour * (1 - _armourPenetration))/((armourPenetration * (1 - armourPenetration)) + (1 / _targetArmourEffectiveness))));
            }

    //deals damage to collided player
    public void DealDamage(GameObject _target, float _finalDamage) {
        if (_target.GetComponent<CharacterStatsManager>() != null) {
            _target.GetComponent<CharacterStatsManager>().TakeDamage(_finalDamage);
        }
    }

    public void DestroyDamager() {
        Destroy(gameObject);
    }

    public void SetStats(GameObject _holder, float _attack, float _armourPenetration, float _damage, string _targetType) {
        holder = _holder;
        attack = _attack;
        armourPenetration = _armourPenetration;
        damage = _damage;
        targetType = _targetType;
    }

    public void SetAttackerStats(float _attack, float _armourPenetration) {
        attack = _attack;
        armourPenetration = _armourPenetration;
    }

    public void SetTargetStats(float _targetArmour, float _targetArmourEffectiveness) {
        targetArmour = _targetArmour;
        targetArmourEffectiveness = _targetArmourEffectiveness;
    }
}
