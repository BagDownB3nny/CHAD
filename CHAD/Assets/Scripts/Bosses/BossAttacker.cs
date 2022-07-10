using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacker : MonoBehaviour
{

    // Attacker trackers
    private bool isPrimaryAttacking;
    private float nextPrimaryAttack;
    [SerializeField]
    private float primaryAttackCooldown;

    private bool isSecondaryAttacking;
    private float nextSecondaryAttack;
    [SerializeField]
    private float secondaryAttackCooldown;

    // Scripts
    private BossStatsManager stats;
    private BossMover mover;

    // Current weapons
    private GameObject primaryAttack;
    private GameObject secondaryAttack;

    [SerializeField]
    private List<GameObject> primaryWeapons;
    [SerializeField]
    private List<GameObject> secondaryWeapons;

    private bool isActive = false;

    public void StartBossFight()
    {
        isActive = true;
        isPrimaryAttacking = false;
        isSecondaryAttacking = false;
    }

    public void EndAttack(string attackType)
    {
        if (attackType == "primary")
        {
            isPrimaryAttacking = false;
            nextPrimaryAttack = primaryAttackCooldown;
        }
        if (attackType == "secondary")
        {
            isSecondaryAttacking = false;
            nextSecondaryAttack = secondaryAttackCooldown;
        }
    }

    public void Update()
    {
        if (isActive && NetworkManager.gameType == GameType.Server)
        {
            if (!isPrimaryAttacking)
            {
                if (nextPrimaryAttack < 0)
                {
                    int chosenPrimaryAttack = ChoosePrimaryAttack();
                    SetPrimaryAttack(chosenPrimaryAttack);
                    ServerSend.SetBossAttack("primary", chosenPrimaryAttack);
                } else
                {
                    nextPrimaryAttack -= Time.deltaTime;
                }
            }
            if (!isSecondaryAttacking)
            {
                if (nextSecondaryAttack < 0)
                {
                    int chosenSecondaryAttack = ChooseSecondaryAttack();
                    SetSecondaryAttack(chosenSecondaryAttack);
                    ServerSend.SetBossAttack("secondary", chosenSecondaryAttack);
                } else
                {
                    nextSecondaryAttack -= Time.deltaTime;
                }
            }
        }
    }

    public void ReceiveSetBossAttack(string _attackType, int _attack)
    {
        if (_attackType == "primary")
        {
            ReceiveSetPrimaryAttack(_attack);
        } else if (_attackType == "secondary")
        {
            ReceiveSetSecondaryAttack(_attack);
        }
    }

    private void SetPrimaryAttack(int _primaryAttack)
    {
        primaryAttack = Instantiate(primaryWeapons[_primaryAttack], transform.position, Quaternion.identity);
        primaryAttack.GetComponent<BossRangedWeapon>().holder = gameObject;
        isPrimaryAttacking = true;
    }

    public void ReceiveSetPrimaryAttack(int _primaryAttack)
    {
        SetPrimaryAttack(_primaryAttack);
    }

    private void SetSecondaryAttack(int _secondaryAttack)
    {
        secondaryAttack = Instantiate(secondaryWeapons[_secondaryAttack], transform.position, Quaternion.identity);
        secondaryAttack.GetComponent<BossRangedWeapon>().holder = gameObject;
        isSecondaryAttacking = true;
    }

    public void ReceiveSetSecondaryAttack(int _secondaryAttack)
    {
        SetSecondaryAttack(_secondaryAttack);
    }

    public void ReceiveAttack(string affectedGun, string projectileRefId, float projectileDirectionRotation)
    {
        if (affectedGun == BossWeaponType.primary.ToString() && primaryAttack != null)
        {
            primaryAttack.GetComponent<WeaponShooter>().ReceiveShoot(
                    projectileRefId, projectileDirectionRotation);
        }
        else if (affectedGun == BossWeaponType.secondary.ToString() && secondaryAttack != null)
        {
            secondaryAttack.GetComponent<WeaponShooter>().ReceiveShoot(
                    projectileRefId, projectileDirectionRotation);
        }
    }

    private int ChoosePrimaryAttack()
    {
        return Random.Range(0, primaryWeapons.Count);
    }

    private int ChooseSecondaryAttack()
    {
        return Random.Range(0, secondaryWeapons.Count);
    }
}
