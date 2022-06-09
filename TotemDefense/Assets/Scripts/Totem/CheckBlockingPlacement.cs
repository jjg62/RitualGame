using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Uses a trigger zone to check if an enemy is blocking totem placement
public class CheckBlockingPlacement : MonoBehaviour
{
    private int enemiesOccupying = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            //Add one to enemies in the zone
            enemiesOccupying++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            //Remove one from enemies in the zone
            enemiesOccupying--;
        }
    }

    public bool IsEnemyBlocking()
    {
        //Can only place when there are no enemies in the zone
        return enemiesOccupying > 0;
    }

}
