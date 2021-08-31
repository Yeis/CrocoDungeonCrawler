using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Random = UnityEngine.Random;
using UnityEngine;
[System.Serializable]

public class Wave {
    public string waveName;
    public int noOfEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
}

public class Spawner : MonoBehaviour {
    public Wave[] waves;
    public Transform[] spawnPoints;
    public GameObject difficultyObject;
    public GameObject bossObject;

    private Wave currentWave;
    private int currentWaveno;
    private float nextSpawntime;
    public bool isInCombat = false;
    private bool canSpawn = true;
    private bool isBossEncounter = false;
    private bool bossIsVulnerable = false;
    private DifficultyController difficultyController;
    private Boss bossController;

    public int firstBossWaveSize = 1;
    public int secondBossWaveSize = 20;
    public int thirdBossWaveSize = 30;

    private int currentWaveSize = 0;
    public BossWave currentBossWave = BossWave.easyWave;

    void Awake() {
        difficultyController = difficultyObject.GetComponent<DifficultyController>();
        bossController = bossObject.GetComponent<Boss>();
    }

    public void startEncounter(int noOfEnemiesToSpawn, bool bossEncounter) {
        // set how many crocos to spawn
        isBossEncounter = bossEncounter;
        currentWaveno = 0;
        if (isBossEncounter) {
            // set up boss waves
            waves[0].noOfEnemies = firstBossWaveSize;
            waves[1].noOfEnemies = secondBossWaveSize;
            waves[2].noOfEnemies = thirdBossWaveSize;
        } else {
            // Normal behavior
            waves[currentWaveno].noOfEnemies = noOfEnemiesToSpawn;
        }

        isInCombat = true;
    }

    public void stopEncounter() {
        isInCombat = false;

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            // TODO: Enemy die animation
            Destroy(enemy);
        }
    }

    private void Update() {
        if (isInCombat) {
            currentWave = waves[currentWaveno];
            spawnWave();
            GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (totalEnemies.Length == 0 && !canSpawn && currentWaveno + 1 != waves.Length && !bossIsVulnerable) {
                if (isBossEncounter) {
                    difficultyController.displayBossComboSequence(currentBossWave);
                    bossIsVulnerable = true;
                } else {
                    currentWaveno++;
                    canSpawn = true;
                }
            }
        }
    }

    public void restartCurrentWave() {
        currentWave.noOfEnemies = firstBossWaveSize;
        canSpawn = true;
        bossController.Chuckle();
        bossController.Attack();
        bossIsVulnerable = false;
    }

    void spawnWave() {
        if (canSpawn && nextSpawntime < Time.time) {
            GameObject Enemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
            int random = Random.Range(0, spawnPoints.Length);
            Transform randomPoint = spawnPoints[random];
            GameObject newEnemy = Instantiate(Enemy, randomPoint.position, Quaternion.identity);
            newEnemy.GetComponent<SpriteRenderer>().sortingOrder = randomPoint.gameObject.GetComponent<SpawnPoint>().spriteLayer;
            newEnemy.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = randomPoint.gameObject.GetComponent<SpawnPoint>().spriteLayer;
            currentWave.noOfEnemies--;
            nextSpawntime = Time.time + currentWave.spawnInterval;

            if (currentWave.noOfEnemies == 0) {
                canSpawn = false;

                if (isBossEncounter) {
                    // 
                }
            }
        }
    }

    public void advanceBossWave() {
        switch (currentBossWave) {
            case BossWave.easyWave:
                currentBossWave = BossWave.mediumWave;
                break;
            case BossWave.mediumWave:
                currentBossWave = BossWave.hardWave;
                break;
        }

        currentWave.noOfEnemies = firstBossWaveSize;
        canSpawn = true;
        bossIsVulnerable = false;

    }

}

public enum BossWave {
    easyWave = 0, mediumWave = 1, hardWave = 3
}
