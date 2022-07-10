using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacker : MonoBehaviour
{
    public bool isPrimaryAttacking;
    public bool isSecondaryAttacking;

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

    public void Update()
    {
        if (isActive && NetworkManager.gameType == GameType.Server)
        {
            if (!isPrimaryAttacking)
            {
                int chosenPrimaryAttack = ChoosePrimaryAttack();
                SetPrimaryAttack(chosenPrimaryAttack);
                ServerSend.SetPrimaryAttack(chosenPrimaryAttack);
            }
            if (!isSecondaryAttacking)
            {
                //GameObject chosenSecondaryAttack = ChooseSecondaryAttack();
                //secondaryAttack = Instantiate(chosenSecondaryAttack);
            }
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

    public void ReceiveAttack(string affectedGun, string projectileRefId, float projectileDirectionRotation)
    {
        Debug.Log("Boss attacker receives attack");
        Debug.Log("server sent: " + affectedGun);
        Debug.Log("boss gun: " + BossWeaponType.primary.ToString());
        if (affectedGun == BossWeaponType.primary.ToString() && primaryAttack != null)
        {
            Debug.Log("Boss attacker recognises primary attack");
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

    private GameObject ChooseSecondaryAttack()
    {
        return secondaryWeapons[Random.Range(0, secondaryWeapons.Count)];
    }
}
