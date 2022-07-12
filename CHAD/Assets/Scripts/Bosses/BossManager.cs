using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossAttacks
{

}

public class BossManager : MonoBehaviour
{

    public static BossManager instance;
    private bool isActive = false;

    // Reference to the boss scripts
    public BossAttacker bossAttacker;
    public BossMover bossMover;

    // Health and phase statuses
    [SerializeField]
    private int maxHealth;
    private int health;
    private int phaseChangeThreshold;

    private void Awake()
    {
        instance = this;
        bossMover = GetComponent<BossMover>();
        bossAttacker = GetComponent<BossAttacker>();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void StartBossFight()
    {
        isActive = true;
        health = maxHealth;
        bossAttacker.StartBossFight();
        bossMover.StartBossFight();
    }

    public int CurrentPhase()
    {
        if (health > phaseChangeThreshold)
        {
            return 1;
        } else
        {
            return 2;
        }
    }
}
