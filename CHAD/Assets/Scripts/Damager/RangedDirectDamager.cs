using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDirectDamager : DirectDamager
{
    ProjectileStatsManager projectileStatsManager;

    void Start()
    {
        projectileStatsManager = gameObject.GetComponent<ProjectileStatsManager>();
    }

    private void OnTriggerEnter2D(Collider2D _collider) {
        if (NetworkManager.gameType == GameType.Server) {
            if (_collider.CompareTag(projectileStatsManager.targetType)) {
                float damageDealt = CalculateDamageDealt(projectileStatsManager.damage, projectileStatsManager.attack);
                DealDamage(_collider.gameObject, damageDealt);
                DestroyDamager();
            }
        }
    }

    public override float CalculateDamageDealt(float _rawDamage, float _attack) {
        return _rawDamage * _attack;
    }

    //deals damage to collided player
    public override void DealDamage(GameObject _target, float _damageDealt) {
        if (_target.GetComponent<CharacterStatsManager>() != null) {
            _target.GetComponent<CharacterStatsManager>().TakeDamage(_damageDealt, projectileStatsManager.armourPenetration);
        }
    }

    public override void DestroyDamager() {
        //calls DestroyProjectile because a RangedDirectDamager is attached to a projectile
        ServerSend.DestroyDamageDealer(gameObject.GetComponent<DamageDealerStatsManager>().damageDealerRefId);
        Destroy(gameObject);
    }
}
