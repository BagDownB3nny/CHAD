using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStatsManager : MonoBehaviour
{
    //Scripts needed
    ProjectileMovement movementScript;
    RangedDirectDamager damagerScript;

    public int projectileRefId;

    private void Awake() {
        movementScript = gameObject.GetComponent<ProjectileMovement>();
        damagerScript = gameObject.GetComponent<RangedDirectDamager>();
    }

    private void Start() {
        UpdateMovementStats();
        UpdateDamageStats();
    }

    [Header("Projectile Parameters")]
    public GameObject holder;
    public float attack;
    public float armourPenetration;
    public float speed;
    public float damage;
    public float range;
    public string targetType;
    public GameObject origin;
    public Vector3 originLocationVector;
    public Vector3 directionVector;
    public float rotationOffset;

    public void UpdateMovementStats() {
        movementScript.SetStats(speed, range, origin, originLocationVector, directionVector, rotationOffset);    
    }

    public void UpdateDamageStats() {
        damagerScript.SetStats(holder, attack, armourPenetration, damage, targetType);
    }

    public void SetStats(GameObject _holder, float _attack, float _armourPenetration, float _speed, float _damage, float _range, string _targetType, GameObject _origin, 
            Vector3 _originLocationVector, Vector3 _directionVector, float _rotationOffset) {
                holder = _holder;
                attack = _attack;
                armourPenetration = _armourPenetration;
                speed = _speed;
                damage = _damage;
                range = _range;
                targetType = _targetType;
                origin = _origin;
                originLocationVector = _originLocationVector;
                directionVector = _directionVector;
                rotationOffset = _rotationOffset;
    }
}
