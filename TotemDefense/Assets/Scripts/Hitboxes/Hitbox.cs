using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Collision area which damages entities of a given type
public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private float duration;
    public int damage = 1;

    [SerializeField]
    private string target;
    private Type t;

    [SerializeField]
    private bool hitOnce; //If true, hitbox will hit ONE enemy exactly once
    private bool canHit;
    [SerializeField]
    private bool hitEachOnce; //If true, hitbox will hit EACH enemy a maximum of 1 time
    private List<IHitable> enemiesHit;

    [SerializeField]
    private String hitSound;


    public float hitstunDuration;
    public float knockbackDistance;

    public bool transcendHitstun;

    [HideInInspector]
    public Totem sourceTotem;


    private void Start()
    {
        //Use the target string to get the type (implementing IHitable) of entity this hitbox will hit (e.g. Totem, Enemy)
        t = Type.GetType(target);
        Invoke("End", duration); //Destroy hitbox after duration
        canHit = true;
        enemiesHit = new List<IHitable>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Check if each object in range is an enemy
        IHitable enemy = (IHitable)collision.GetComponent(t);
        bool alreadyHit = hitEachOnce && enemiesHit.Contains(enemy);
        //If hitbox can still hit this enemy
        if(enemy != null && canHit && !alreadyHit)
        {
            enemy.Hit(this);
            enemiesHit.Add(enemy);
            if (hitOnce)
            {
                Destroy(gameObject);
                //canHit = false;
                //this.enabled = false;
            }
        }
    }

    private void End()
    {
        Destroy(gameObject);
    }

    public void PlayHitSound()
    {
        if(hitSound != null) Game.instance.sound.Play(hitSound);
    }
}
