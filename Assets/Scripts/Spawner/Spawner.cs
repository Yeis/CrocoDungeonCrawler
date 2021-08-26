using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Random = UnityEngine.Random;
using UnityEngine;
[System.Serializable]

public class Wave
{
    public string waveName;
    public int noOfEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
    
}

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;

    private Wave currentWave;
    private int currentWaveno;
    private float nextSpawntime;

    private bool canSpawn = true;

    private void Update()
    {
        currentWave = waves[currentWaveno];
        spawnWave();
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (totalEnemies.Length == 0 && !canSpawn && currentWaveno+1 != waves.Length)
        {
            currentWaveno++;
            canSpawn = true;
        }
    }

    void spawnWave()
    {
        if (canSpawn && nextSpawntime < Time.time)
        {
            GameObject Enemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(Enemy, randomPoint.position, Quaternion.identity);
            currentWave.noOfEnemies--;
            nextSpawntime = Time.time + currentWave.spawnInterval;
            if (currentWave.noOfEnemies == 0)
            {
                canSpawn = false;
            }
        }
    }
}
