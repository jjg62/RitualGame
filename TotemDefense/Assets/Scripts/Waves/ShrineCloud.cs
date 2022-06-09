using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cloud which hints at upcoming enemies during build phase
public class ShrineCloud : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] slots;

    [SerializeField]
    private Enemy[] example;

    public void Setup(Enemy[] enemies)
    {
        int length = enemies != null ? enemies.Length : 0;
        for(int i = 0; i < 3; i++)
        {
            //Clouds have 3 slots, so activate those which are needed
            if(i < length)
            {
                slots[i].gameObject.SetActive(true);
                //Set sprite of slot to enemy's sprite
                slots[i].sprite = enemies[i].GetComponent<SpriteRenderer>().sprite;
                slots[i].transform.localPosition = new Vector2((length-1) / -2f + i, 0f);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
            
        }
    }
}
