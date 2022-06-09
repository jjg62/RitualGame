using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Change enemy's form after it attacks a certain amountmt of times (e.g. runs out of ammo)
public class EnemyAttackChange : MonoBehaviour
{

    [SerializeField]
    private Enemy newPhase; //New form to transform into

    private int attacks;
    [SerializeField]
    private int attacksToChange;


    public void Attacked()
    {
        attacks++;
        if(attacks >= attacksToChange)
        {
            //Transform
            Enemy old = GetComponent<Enemy>();
            Enemy e = Instantiate(newPhase);
            //Transfer data to new form
            e.transform.position = transform.position;
            e.health = old.health;
            e.wave = old.wave;
            Destroy(gameObject); //Destroy this old form
        }
    }
}
