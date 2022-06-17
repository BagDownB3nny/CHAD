using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySpawner : MonoBehaviour
{
    // To keep track of enemies in level
    public List<int> enemiesPerLevel;
    public int totalEnemiesToSpawn;
    public int enemiesLeftToSpawn;
    public int enemiesAlive;
    public float timeToNextSpawn;
    public int[][] playerBounds;

    // To check if spawner should start spawning
    public bool isSpawning;

    private void Update()
    {
        if (NetworkManager.gameType == GameType.Server)
        {
            /* Conditions for enemy spawning:
             * 1. EnemySpawner has been made active
             * 2. enemiesLeftToSpawn > 0
             * 3. timeToNextSpawn <= 0
             * 4. enemiesAlive < 0.3 * totalEnemiesToSpawn (the map does not contain too many enemies)
             */
            if (isSpawning && enemiesLeftToSpawn > 0 && timeToNextSpawn <= 0 && enemiesAlive < 0.3 * totalEnemiesToSpawn)
            {
                int[] coordinates = generateCoordinates();
                Enemies enemy = generateRandomEnemy();
                SpawnEnemy(enemy, coordinates);
                timeToNextSpawn = generateNextSpawnTime();
            } else if (timeToNextSpawn > 0)
            {
                timeToNextSpawn -= Time.deltaTime;
            }
        }
    }

    public void StartSpawning()
    {
        isSpawning = true;
        totalEnemiesToSpawn = enemiesPerLevel[GameManager.instance.currentLevel] * Server.NumberOfPlayers;
        enemiesLeftToSpawn = totalEnemiesToSpawn;
        timeToNextSpawn = 5.0f;
    }

    #region SpawnEnemy
    /* Instantiates an enemy
     * Creates an enemyId (based on the current enemies that have yet to be spawned)
     * Adds enemy to GameManager dictionary
     * Decrement enemiesToSpawn
     * Increment enemiesAlive
     */
    public void SpawnEnemy(Enemies _enemy, int[] coordinates)
    {
        GameObject enemy = Instantiate(GameManager.instance.enemyPrefabs[(int)_enemy],
                CoordinatesToWorldPoint(coordinates), Quaternion.identity);
        string enemyId = enemiesLeftToSpawn.ToString();
        GameManager.instance.enemies.Add(enemyId, enemy);
        enemiesLeftToSpawn -= 1;
        enemiesAlive += 1;
    }

    public void ReceiveSpawnEnemy(string enemyId, int enemyType, Vector2 position)
    {

    }
    #endregion

    public Vector2 CoordinatesToWorldPoint(int[] coordinates)
    {
        return new Vector2(0, 0);
    }

    public int[] generateCoordinates()
    {
        /* TODO: 
         * Set player bounds
         * Generate a random coordinate
         * Check if out of player bounds && on floor tile
         * return coordinate
         */
        return null;
    }

    public Enemies generateRandomEnemy()
    {
        int randomInt = Random.Range(1, 11);
        if (randomInt <= 4)
        {
            return (Enemies)GameManager.instance.currentLevel;
        }
        else if (randomInt <= 7)
        {
            return (Enemies)GameManager.instance.currentLevel + 1;
        }
        else if (randomInt <= 9)
        {
            return (Enemies)GameManager.instance.currentLevel + 2;
        }
        else
        {
            return (Enemies)GameManager.instance.currentLevel + 3;
        }
    }

    public float generateNextSpawnTime()
    {
        // If there are few enemies, spawn enemies more quickly
        int enemyMinimumThreshHold = 5;
        float fastSpawnInterval = 0.2f;
        if (enemiesAlive <= enemyMinimumThreshHold)
        {
            return fastSpawnInterval;
        }

        // Spawn enemies more quickly as more enemies have spawned
        float enemiesToSpawnRatio = enemiesLeftToSpawn / totalEnemiesToSpawn;
        float[] spawnIntervals = new float[] { 0.4f, 0.7f, 1.0f };
        float[] timeIntervals = new float[] { 2.0f, 5.0f, 7.0f };
        for (int i = 0; i < spawnIntervals.Length; i++)
        {
            if (enemiesToSpawnRatio <= spawnIntervals[i])
            {
                return timeIntervals[i] / Server.NumberOfPlayers;
            }
        }
        return 0f;
    }
}
