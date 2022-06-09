using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Change enemy form after taking damage
public class EnemyDamageChange : MonoBehaviour, IHitstunListener
{
    private Enemy enemy; //The enemy this belongs to

    [SerializeField]
    private int hitsRequired;
    private int hits;

    private bool done;

    //Animations played when transitioning to new form
    [SerializeField]
    private GameObject exitAnimation; 
    [SerializeField]
    private EnemySpawnAnimation newSpawn;

    [SerializeField]
    private float newSpawnOffset; //Position of new form spawn

    [SerializeField]
    private float spawnDelay; //Time delay before new form spawns


    //On hit
    public void Hitstun(Hitbox hb)
    {
        if (!done)
        {
            hits += hb.damage;
            if (hits >= hitsRequired)
            {
                //Start transforming
                done = true;

                GameObject animation = Instantiate(exitAnimation);
                animation.transform.position = transform.position;
                animation.transform.localScale = new Vector2(Mathf.Sign(transform.position.x), 1);

                gameObject.SetActive(false);
                Invoke("Spawn", spawnDelay); //Spawn new form after delay
            }
        }
    }

    //Destroy current form and spawn new one
    private void Spawn()
    {
        EnemySpawnAnimation spawn = Instantiate(newSpawn);
        spawn.wave = enemy.wave;
        spawn.transform.position = new Vector2(transform.position.x + enemy.GetDirection() * newSpawnOffset, transform.position.y);

        Destroy(gameObject);
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemy.AddListener(this); //Listen for hits
        done = false;
    }

}
