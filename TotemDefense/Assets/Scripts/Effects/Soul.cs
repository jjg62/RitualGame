using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Object which is created to give player another soul (currency) with a short animation
public class Soul : MonoBehaviour
{
    private Vector2 start;
    private Vector2 target;
    private float time;
    

    private void Start()
    {
        //Target position is the position of soul counter on UI
        target = Camera.main.ScreenToWorldPoint(BattleUI.instance.soulCounter.position);
        start = transform.position;
        time = 0f;
    }

    
    private void Update()
    {
        //Lerp position towards soul counter
        transform.position = Vector2.Lerp(start, target, time/0.4f);
        time += Time.deltaTime;

        //After a short duration, add 1 to soul counter
        if (time > 0.4f)
        {
            Game.instance.AddSouls(1);
            Game.instance.sound.Play("Soul-Collected");
            Destroy(gameObject);
        }
    }

}
