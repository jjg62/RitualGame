using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Totem needs to get a certain amount of enemy hits
public class ConditionGetHits : MoveCondition, INotifyOnHit
{
    [SerializeField]
    private int requiredHits;

    //Whenever an enemy is hit
    public void OnHit(Enemy e)
    {
        //Increment progress counter
        moveProgress = Mathf.Min(moveProgress + 1 / (requiredHits / 1.0f), 1.0f);
        UpdateCanMove();
    }

    public override void ResetProgress()
    {
        //Reset hits to 0
        moveProgress = 0f;
        UpdateCanMove();
    }

    public override string Description()
    {
        return "Hit Enemies " + requiredHits + " times";
    }
}
