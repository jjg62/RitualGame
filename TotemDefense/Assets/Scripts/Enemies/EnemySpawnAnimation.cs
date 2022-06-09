using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Play an animation before creating enemy instance
public class EnemySpawnAnimation : MonoBehaviour
{
    [SerializeField]
    private float after; //When to spawn enemy

    [SerializeField]
    private Enemy enemySpawn; //Enemy to spawn

    [HideInInspector]
    public Wave wave; //Store wave so it can be passed to enemy instance

    private void Start()
    {
        GetComponent<SpriteRenderer>().flipX = transform.position.x < 0; //Change direction facing
        Invoke("Spawn", after);
    }

    //Destroy this animataion and spawn the enemy instance
    private void Spawn()
    {
        Destroy(gameObject);
        Enemy e = Instantiate(enemySpawn);

        e.wave = wave;
        e.transform.position = transform.position;
    }
}
