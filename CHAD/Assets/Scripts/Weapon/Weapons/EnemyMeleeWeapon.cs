using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMeleeWeapon : MeleeWeapon
{
    void Update()
    {
        if (NetworkManager.gameType == GameType.Server) {
            if (CanAttack() && currentDamageDealer == null) {
                Attack();
                Debug.Log("White Dude Attacking on Server");
            }
            timeToNextAttack -= Time.deltaTime;
        }
    }
}
