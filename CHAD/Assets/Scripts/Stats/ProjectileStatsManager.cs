using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStatsManager : MonoBehaviour
{
    public string projectileRefId;

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
    public Vector3 projectileDirectionVector;
    public float rotationOffset;

    public void SetStats(string _projectileRefId, GameObject _weaponHolder, RangedWeapon _rangedWeapon, GameObject _origin, 
            Vector3 _projectileDirectionVector, float _rotationOffset) {
                holder = _weaponHolder;
                CharacterStatsManager characterStatsManager = _weaponHolder.GetComponent<CharacterStatsManager>();
                attack = characterStatsManager.attack;
                armourPenetration = characterStatsManager.armourPenetration;
                speed = _rangedWeapon.speed;
                damage = _rangedWeapon.damage;
                range = _rangedWeapon.range;
                targetType = _rangedWeapon.targetType;
                origin = _origin;
                originLocationVector = _origin.transform.position;
                projectileDirectionVector = _projectileDirectionVector;
                rotationOffset = _rotationOffset;
    }
}
