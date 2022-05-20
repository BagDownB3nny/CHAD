using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealerStatsManager : MonoBehaviour
{
    //Scripts needed
    MeleeDirectDamager damagerScript;

    [Header("Damage Dealer Parameters")]
    public GameObject holder;
    public float attack;
    public float armourPenetration;
    public float damage;
    public string targetType;
    public GameObject origin;

    private void Awake() {
        damagerScript = gameObject.GetComponent<MeleeDirectDamager>();
    }

    private void Start() {
        UpdateDamageStats();
    }

    public void UpdateDamageStats() {
        damagerScript.SetStats(holder, attack, armourPenetration, damage, targetType);
    }

    public void SetStats(GameObject _holder, float _attack, float _armourPenetration, float _damage, 
            string _targetType, GameObject _origin) {
                holder = _holder;
                attack = _attack;
                armourPenetration = _armourPenetration;
                damage = _damage;
                targetType = _targetType;
                origin = _origin;
    }
}
