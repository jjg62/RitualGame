using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Information about one enemy's spawn - which enemy, when and where to spawn them
[System.Serializable]
public class SpawnInfo
{
    public EnemySpawnAnimation enemy;
    public float spawnDelay;
    public Vector2 spawnLocation;
}
