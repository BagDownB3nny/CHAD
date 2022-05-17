using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthScript : MonoBehaviour
{
    // Start is called before the first frame update
    public abstract void TakeDamage(float damage, GameObject bullet);

    public abstract void Die();
}
