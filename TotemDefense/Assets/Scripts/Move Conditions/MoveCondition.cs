using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Children of this describe conditions needed to be met to move a totem to other side
public abstract class MoveCondition : MonoBehaviour
{
    public float moveProgress; //Number between 0-1. If >=1, can move totem
    public abstract void ResetProgress(); //After moving, do whatever needs to be done to reset the condition
    public abstract string Description(); //String describing how to meet the condition, displayed on UI

    private Totem totem;
    private SpriteRenderer spr;
    private bool canMove;

    [SerializeField]
    private Color glowColour;
    private Color normalColour = Color.white;

    private RitualBar ritualBar; //Interface object displaying progress

    [SerializeField]
    private Soul soulPref;

    protected void Awake()
    {
        totem = GetComponent<Totem>();
        spr = GetComponent<SpriteRenderer>();
        ritualBar = GetComponentInChildren<RitualBar>(true);
        
        if(moveProgress >= 1.0f)
        {
            canMove = true;
            StartCoroutine(GlowEffect());
        }
    }

    //Call this when progress is changed to check if totem can now move
    protected void UpdateCanMove()
    {
        if(!canMove && moveProgress >= 1.0f)
        {
            canMove = true;

            //Start glowing and drop a soul when condition met for first time
            Instantiate(soulPref).transform.position = transform.position;
            StartCoroutine(GlowEffect());
        }
        else if(canMove && moveProgress < 1.0f)
        {
            //Reset sprite colour if progress is reset
            canMove = false;
            spr.color = normalColour;
        }

        //Update interface
        ritualBar.ShowAnimation(moveProgress);
        totem.UpdateInfoPanel();
    }


    //Visual effect when totem can be moved
    IEnumerator GlowEffect()
    {
        float t = 0f;
        const float PERIOD = 0.5f;
        const float MAX_ALPHA = 0.75f;
        while (canMove)
        {
            //Lerp towards glow colour then back to normal colour
            t += Time.deltaTime;
            if(t < PERIOD / 2f)
            {
                spr.color = Color.Lerp(normalColour, glowColour, t / (PERIOD/2f));
            }
            else if(t < PERIOD){
                spr.color = Color.Lerp(glowColour, normalColour, (t - PERIOD / 2f) / (PERIOD/2f));
            }
            else
            {
                t = 0;
            }
            yield return null;
        }
    }
}
