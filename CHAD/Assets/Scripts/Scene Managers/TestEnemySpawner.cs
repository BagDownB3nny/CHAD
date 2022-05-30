using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawner : EnemySpawner
{
    private void Update() {
        if (NetworkManager.gameType == GameType.Server) {
            SpawnEnemy();
        }
    }

    public override void SpawnEnemy() {
        if(GameManager.instance.players.Count > 0 && timeToNextSpawn <= 0) {
            GameObject enemy = Instantiate(GameManager.instance.enemyPrefabs[(int) enemyId], transform.position, Quaternion.identity);
            
            string enemyRefId = string.Format("{0}{1}.{2}", spawnerInitials, spawnerId, localEnemyRefId);
            string spawnerRefId = string.Format("{0}{1}", spawnerInitials, spawnerId);
            GameManager.instance.enemies.Add(enemyRefId, enemy);
            enemy.GetComponent<EnemyStatsManager>().characterRefId = enemyRefId;
            ServerSend.SpawnEnemy(spawnerRefId, enemyRefId, (int) enemyId, transform.position);
            localEnemyRefId++;

            timeToNextSpawn = spawnInterval;
        }
        timeToNextSpawn -= Time.deltaTime;
    }

    public override void ReceiveSpawnEnemy(string _enemyRefId, int _enemyId, Vector2 _position) {
        GameObject enemySpawned = Instantiate(GameManager.instance.enemyPrefabs[_enemyId], _position, Quaternion.identity);
        enemySpawned.GetComponent<EnemyStatsManager>().characterRefId = _enemyRefId;
        GameManager.instance.enemies.Add(_enemyRefId, enemySpawned);
    }
}
