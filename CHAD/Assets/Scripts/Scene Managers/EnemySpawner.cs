using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    // To keep track of enemies in level
    public static List<int> enemiesPerLevel;
    public int totalEnemiesToSpawn;
    public int enemiesLeftToSpawn;
    public int enemiesAlive;
    public int enemiesKilled;
    public float timeToNextSpawn;
    public List<int[]> playerCoords;

    // To check if spawner should start spawning
    public bool isSpawning;

    //Reference to the map
    public GameObject map;

    //Static reference to the enemy spawner
    public static EnemySpawner instance;

    //Reference to the Objective Text UI
    public GameObject objectiveText;

    private void Awake()
    {
        enemiesPerLevel = new List<int>(new int[] { 100, 200, 300, 450, 600 });
        instance = this;
        EnemyDeath.onEnemyDeath += OnEnemyDeath;
        objectiveText = GameUIManager.instance.objectiveText;
    }

    private void OnDestroy()
    {
        instance = null;
    }

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
                Debug.Log("coordinates: " + coordinates);
                if (coordinates != null)
                {
                    Enemies enemy = generateRandomEnemy();
                    SpawnEnemy(enemy, coordinates);
                    timeToNextSpawn = generateNextSpawnTime();
                }
            } else if (timeToNextSpawn > 0)
            {
                timeToNextSpawn -= Time.deltaTime;
            }
        }
    }

    public void StartSpawning()
    {
        isSpawning = true;
        Debug.Log("LEVEL " + GameManager.instance.currentLevel);
        Debug.Log(enemiesPerLevel.Count);
        totalEnemiesToSpawn = (enemiesPerLevel[GameManager.instance.currentLevel++]) * Server.NumberOfPlayers;
        enemiesLeftToSpawn = totalEnemiesToSpawn;
        timeToNextSpawn = 5.0f;

        objectiveText.SetActive(true);
        objectiveText.GetComponent<TextMeshProUGUI>().text = "KILL ALL ENEMIES\n" + enemiesKilled + "/" + totalEnemiesToSpawn + " KILLED";
    }

    #region SpawnEnemy
    /* Instantiates an enemy
     * Creates an enemyId (based on the current enemies that have yet to be spawned)
     * Adds enemy to GameManager dictionary
     * Decrement enemiesToSpawn
     * Increment enemiesAlive
     */
    public void SpawnEnemy(Enemies _enemy, int[] _coordinates)
    {
        MapGenerator mapGenerator = map.GetComponent<MapGenerator>();
        GameObject enemy = Instantiate(GameManager.instance.enemyPrefabs[(int)_enemy],
                mapGenerator.CoordToWorldPoint(_coordinates[0], _coordinates[1]), Quaternion.identity);
        string enemyId = enemiesLeftToSpawn.ToString();
        enemy.GetComponent<EnemyStatsManager>().characterRefId = enemyId;
        GameManager.instance.enemies.Add(enemyId, enemy);
        enemiesLeftToSpawn -= 1;
        enemiesAlive += 1;
        ServerSend.SpawnEnemy(enemyId, _enemy, mapGenerator.CoordToWorldPoint(_coordinates[0], _coordinates[1]));
    }


    public void ReceiveSpawnEnemy(string _enemyId, int _enemyType, Vector2 _position)
    {
        GameObject enemy = Instantiate(GameManager.instance.enemyPrefabs[(int)_enemyType],
                _position, Quaternion.identity);
        enemy.GetComponent<EnemyStatsManager>().characterRefId = _enemyId;
        GameManager.instance.enemies.Add(_enemyId, enemy);
    }
    #endregion

    public int[] generateCoordinates()
    {
        /* Set player bounds
         * Generate a random coordinate
         * Check if out of player bounds && on floor tile
         * return coordinate
         */
        MapGenerator mapGenerator = map.GetComponent<MapGenerator>();
        FindPlayerBounds();
        // Try to find a random coordinate that is not near a player && is a floor
        int tries = 5;
        for (int i = 0; i < tries; i++)
        {
            int[] randomCoordinate = new int[2] { Random.Range(0, mapGenerator.width), Random.Range(0, mapGenerator.height) };

            if (IsWithinPlayerBounds(randomCoordinate))
            {
                continue;
            } else if (mapGenerator.floorMap[randomCoordinate[0], randomCoordinate[1]] == 0)
            {
                return randomCoordinate;
            }
        }
        return null;
    }

    private void FindPlayerBounds()
    {
        MapGenerator mapGenerator = map.GetComponent<MapGenerator>();
        playerCoords = new List<int[]>();
        foreach (GameObject player in GameManager.instance.players.Values)
        {
            playerCoords.Add(mapGenerator.WorldPointToCoord(player.transform.position));
        }
    }

    private bool IsWithinPlayerBounds(int[] randomCoord)
    {
        // Getting MapGenerator and Camera
        MapGenerator mapGenerator = map.GetComponent<MapGenerator>();
        Camera camera = CameraMotor.instance.gameObject.GetComponent<Camera>();

        // Finding coordinates of the camera corners
        Vector3 cameraBottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        int[] cameraBottomLeftCoord = mapGenerator.WorldPointToCoord(cameraBottomLeft);
        Vector3 cameraTopRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        int[] cameraTopRightCoord = mapGenerator.WorldPointToCoord(cameraTopRight);

        // Setting the PlayerBounds
        int verticalBound = (cameraTopRightCoord[1] - cameraBottomLeftCoord[1]) / 2;
        int horizontalBound = (cameraTopRightCoord[0] - cameraBottomLeftCoord[0]) / 2;

        foreach (int[] playerCoord in playerCoords)
        {
            if (playerCoord[0] < randomCoord[0] && randomCoord[0] < playerCoord[0] + horizontalBound)
            {
                if (playerCoord[1] < randomCoord[1] && randomCoord[1] < playerCoord[1] + verticalBound)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Enemies generateRandomEnemy()
    {
        //TODO: Add more enemies
        //int randomInt = Random.Range(1, 11);
        //if (randomInt <= 4)
        //{
        //    return (Enemies)GameManager.instance.currentLevel;
        //}
        //else if (randomInt <= 7)
        //{
        //    return (Enemies)GameManager.instance.currentLevel + 1;
        //}
        //else if (randomInt <= 9)
        //{
        //    return (Enemies)GameManager.instance.currentLevel + 2;
        //}
        //else
        //{
        //    return (Enemies)GameManager.instance.currentLevel + 3;
        //}
        int randomInt = Random.Range(0, 2);
        return (Enemies)randomInt;
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

    public void OnEnemyDeath(GameObject deadEnemy) {
        enemiesAlive -= 1;
        enemiesKilled += 1;

        if (enemiesKilled >= totalEnemiesToSpawn) {
            objectiveText.GetComponent<TextMeshProUGUI>().text = "FIND THE EXIT";
        } else {
            objectiveText.GetComponent<TextMeshProUGUI>().text = "KILL ALL ENEMIES\n" + enemiesKilled + "/" + totalEnemiesToSpawn + " KILLED";
        }
    }
}
