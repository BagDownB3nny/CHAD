using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{

    protected void Awake()
    {
        BossDeath.onBossDeath += OnBossDeath;
        SetMovement();
        SetAttack();
    }

    public abstract void SetMovement();

    public abstract void SetAttack();

    public void OnDestroy()
    {
        BossDeath.onBossDeath -= OnBossDeath;
    }

    public void OnBossDeath(GameObject boss)
    {
        Destroy(gameObject);
    }
}
