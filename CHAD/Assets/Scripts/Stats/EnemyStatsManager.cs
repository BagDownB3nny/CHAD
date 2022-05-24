using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : CharacterStatsManager
{
    //Scripts needed
    public EnemyMovement movementScript;
    public EnemyWeaponManager weaponManagerScript;

    protected override void Awake() {
        base.Awake();
        movementScript = gameObject.GetComponent<EnemyMovement>();
        weaponManagerScript = gameObject.GetComponent<EnemyWeaponManager>();
    }

    public override void UpdateMovementStats() {
        movementScript.SetStats(speed);    
    }

    public override void UpdateAttackStats() {
        weaponManagerScript.SetAttackStats(attack, armourPenetration);
    }

    public override void UpdateTargetStats(GameObject _damager) {
        _damager.GetComponent<Damager>().SetTargetStats(armour, armourEffectiveness);
    }

    public void SetStats(float _hp, float _attack, float _speed, float _armour, 
            float _armourPenetration, float _targetArmourEffectiveness, float _proficiency) {
                hp = _hp;
                attack = _attack;
                speed = _speed;
                armour = _armour;
                armourPenetration = _armourPenetration;
                armourEffectiveness = _targetArmourEffectiveness;
                proficiency = _proficiency;
            }
}
