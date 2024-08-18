using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyGeneration : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs
    public Transform[] spawnPoints;   // Array of spawn points

    public float spawnInterval = 5f;  // Time between spawns

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Randomly select an enemy prefab
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);

        // Randomly select a spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Instantiate the enemy at the spawn point
        Instantiate(enemyPrefabs[enemyIndex], spawnPoints[spawnIndex].position, Quaternion.identity);
    }
}
