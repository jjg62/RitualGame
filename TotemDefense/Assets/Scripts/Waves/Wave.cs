using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//One wave of enemies
[System.Serializable]
public class Wave
{

    public SpawnInfo[] spawns; //Spawns for this wave
    [Range(0f, 1f)]
    public float clearThreshold; //How many enemies need to be defeated before next wave is spawned

    [HideInInspector]
    public WaveSpawner spawner;

    private int enemiesKilled;
    private float clearProgress;
    private bool waveOver;

    public void StartWave()
    {
        waveOver = false;
        enemiesKilled = 0;
        clearProgress = 0;
    }

    public void EnemyKilled()
    {
        //Increment enemies killed
        enemiesKilled++;
        clearProgress = enemiesKilled / (spawns.Length / 1.0f);
        //If enough killed, spawn next wave
        if(clearProgress >= clearThreshold && !waveOver)
        {
            waveOver = true;
            spawner.NextWave();
        }
    }
}
