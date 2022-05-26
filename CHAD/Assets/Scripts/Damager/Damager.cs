using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damager : MonoBehaviour
{
    public abstract void DealDamage(GameObject _target, float _damageDealt);

    public abstract void DestroyDamager();

    public void ReceiveDestroyDamager() {
        Destroy(gameObject);
    }
}
