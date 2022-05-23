using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawner : MonoBehaviour
{
    [Header("Enemies to Spawn")]
    public Enemies enemyId;

    [Header("Spawner Parameters")]
    public float spawnInterval;
    public float timeToNextSpawn;

    private void Update() {
        if (NetworkManager.gameType == GameType.Server) {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy() {
        if(GameManager.instance.players.Count > 0 && timeToNextSpawn <= 0) {
            GameObject enemy = Instantiate(GameManager.instance.enemyPrefabs[(int) enemyId], transform.position, Quaternion.identity);
            GameManager.instance.SpawnEnemy(enemy, (int) enemyId, transform.position);

            timeToNextSpawn = spawnInterval;
        }
        timeToNextSpawn -= Time.deltaTime;
    }
}
