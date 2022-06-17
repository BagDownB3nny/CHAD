using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySpawner : MonoBehaviour
{
    // To keep track of enemies in level
    public List<int> enemiesPerLevel;
    public int enemiesToSpawn;
    public int enemiesAlive;
    public Time timeToNextSpawn;
    public int[][] playerBounds;

    // To check if spawner should start spawning

    private void Update()
    {
        if (NetworkManager.gameType == GameType.Server)
        {
            // TODO: Add spawning behavior
        }
    }
}
