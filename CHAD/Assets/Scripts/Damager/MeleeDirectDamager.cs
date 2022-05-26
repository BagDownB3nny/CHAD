using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDirectDamager : DirectDamager
{

    DamageDealerStatsManager damageDealerStatsManager;
    void Start()
    {
        damageDealerStatsManager = GetComponent<DamageDealerStatsManager>();
    }
    private void OnTriggerEnter2D(Collider2D _collider) {
        if (NetworkManager.gameType == GameType.Server) {
            if (_collider.CompareTag(damageDealerStatsManager.targetType)) {
                float finalDamage = CalculateDamageDealt(damageDealerStatsManager.damage, damageDealerStatsManager.attack);
                DealDamage(_collider.gameObject, finalDamage);
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
            _target.GetComponent<CharacterStatsManager>().TakeDamage(_damageDealt, damageDealerStatsManager.armourPenetration);
        }
    }

    public override void DestroyDamager() {
        //TODO:
        //calls DestroyDamageDealer because a MeleeDirectDamager is attached to a DamageDealer
        //ServerSend.DestroyProjectile(gameObject.GetComponent<ProjectileStatsManager>().projectileRefId);
        Destroy(gameObject);
    }
}
