using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStatsManager : MonoBehaviour
{
    public int projectileId;

    private void Awake() {

    }

    private void Start() {

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

    public void SetStats(GameObject _weaponHolder, RangedWeapon _rangedWeapon, GameObject _origin, 
            Vector3 _directionVector, float _rotationOffset) {
                holder = _weaponHolder;
                PlayerStatsManager playerStatsManager = _weaponHolder.GetComponent<PlayerStatsManager>();
                attack = playerStatsManager.attack;
                armourPenetration = playerStatsManager.armourPenetration;
                speed = _rangedWeapon.speed;
                damage = _rangedWeapon.damage;
                range = _rangedWeapon.range;
                targetType = _rangedWeapon.targetType;
                origin = _origin;
                originLocationVector = _origin.transform.position;
                directionVector = _directionVector;
                rotationOffset = _rotationOffset;
    }
}
