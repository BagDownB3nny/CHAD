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
        if (isActive)
        {
            if (!isPrimaryAttacking)
            {
                GameObject chosenPrimaryAttack = ChoosePrimaryAttack();
                primaryAttack = Instantiate(chosenPrimaryAttack, transform.position, Quaternion.identity);
                primaryAttack.GetComponent<BossRangedWeapon>().holder = gameObject;
                isPrimaryAttacking = true;
            }
            if (!isSecondaryAttacking)
            {
                //GameObject chosenSecondaryAttack = ChooseSecondaryAttack();
                //secondaryAttack = Instantiate(chosenSecondaryAttack);
            }
        }
    }

    public void ReceiveAttack(string affectedGun, string projectileRefId, float projectileDirectionRotation)
    {
        if (affectedGun == "primary" && primaryAttack != null)
        {
            primaryAttack.GetComponent<WeaponShooter>().ReceiveShoot(
                    projectileRefId, projectileDirectionRotation);
        }
        else if (affectedGun == "secondary" && secondaryAttack != null)
        {
            secondaryAttack.GetComponent<WeaponShooter>().ReceiveShoot(
                    projectileRefId, projectileDirectionRotation);
        }
    }

    private GameObject ChoosePrimaryAttack()
    {
        return primaryWeapons[Random.Range(0, primaryWeapons.Count)];
    }

    private GameObject ChooseSecondaryAttack()
    {
        return secondaryWeapons[Random.Range(0, secondaryWeapons.Count)];
    }
}
