using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Reference ID")]
    public string spawnerInitials;
    public string spawnerId;
    public int localEnemyRefId = 0;

    [Header("Enemies to Spawn")]
    public Enemies enemyId;

    [Header("Spawner Parameters")]
    public float spawnInterval;
    public float timeToNextSpawn;

    public abstract void SpawnEnemy();

    public abstract void ReceiveSpawnEnemy(string _enemyRefId, int _enemyId, Vector2 _position);
}
