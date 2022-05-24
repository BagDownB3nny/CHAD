using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDirectDamager : MonoBehaviour, DirectDamager
{
    ProjectileStatsManager projectileStatsManager;

    void Start()
    {
        projectileStatsManager = gameObject.GetComponent<ProjectileStatsManager>();
    }

    private void OnTriggerEnter2D(Collider2D _collider) {
        if (_collider.CompareTag(projectileStatsManager.targetType)) {
            float damageDealt = CalculateDamageDealt(projectileStatsManager.damage, projectileStatsManager.attack);
            DealDamage(_collider.gameObject, damageDealt);
            DestroyDamager();
        }
    }

    public float CalculateDamageDealt(float _rawDamage, float _attack) {
        return _rawDamage * _attack;
    }

    //deals damage to collided player
    public void DealDamage(GameObject _target, float _damageDealt) {
        if (_target.GetComponent<CharacterStatsManager>() != null) {
            _target.GetComponent<CharacterStatsManager>().TakeDamage(_damageDealt, projectileStatsManager.armourPenetration);
        }
    }

    public void DestroyDamager() {
        Destroy(gameObject);
    }
}
