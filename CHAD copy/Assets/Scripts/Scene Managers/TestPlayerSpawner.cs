using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerSpawner : MonoBehaviour
{
    [Header("Enemies to Spawn")]
    public GameObject Enemy;

    [Header("Spawner Parameters")]
    public float spawnInterval;
    public float timeToNextSpawn;

    private void Update() {
        if(timeToNextSpawn <= 0) {
            GameObject spawned = Instantiate(Enemy, transform.position, Quaternion.identity);
            //spawned.GetComponent<RangedEnemyMovement>().enemy = spawned.GetComponent<Rigidbody2D>();
            timeToNextSpawn = spawnInterval;
        }
        timeToNextSpawn -= Time.deltaTime;
    }
}
