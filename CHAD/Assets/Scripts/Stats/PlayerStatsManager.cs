using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    //Scripts needed
    public PlayerMovement movementScript;
    public PlayerWeaponsManager weaponsManagerScipt;

    [Header("Network Id")]
    public string myId;

    protected override void Awake() {
        base.Awake();
        movementScript = gameObject.GetComponent<PlayerMovement>();
        weaponsManagerScipt = gameObject.GetComponent<PlayerWeaponsManager>();
    }

    public override void UpdateMovementStats() {
        movementScript.SetMovementStats(speed);    
    }

    //call whenever there is a change in attack stats
    public override void UpdateAttackStats() {
        //transfer attack stats to weapon manager, then to weapon then to projectile
        weaponsManagerScipt.SetAttackStats(attack, armourPenetration);
    }

    public override void UpdateTargetStats(GameObject _damager) {
        //can be cany type of damager doesnt matter as long as this script passes the info over
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
