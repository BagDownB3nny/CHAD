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

    private void Update() {
        if (target == null) {
            FindTarget();
        }
    }

    public void FindTarget() {
        if (target == null) {
            GameObject[] gameObjects;
            gameObjects = GameObject.FindGameObjectsWithTag(targetType);
            if (gameObjects.Length > 0) {
                int rand = Random.Range(0, gameObjects.Length);
                target = gameObjects[rand];
            }
        }
    }
}
