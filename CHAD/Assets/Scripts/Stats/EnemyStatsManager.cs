using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : CharacterStatsManager
{
    [Header("Network ID")]
    public string targetType = "Player";

    protected override void Awake() {
        base.Awake();
        characterType = CharacterType.Enemy;
        FindTarget();
    }

    protected void FindTarget() {
        if (target == null) {
            GameObject[] gameObjects;
            gameObjects = GameObject.FindGameObjectsWithTag(targetType);
            if (gameObjects.Length > 0) {
                int rand = Random.Range(0, gameObjects.Length - 1);
                target = gameObjects[rand];
            }
        }
    }
}
