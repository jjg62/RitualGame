using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Totem needs to use its active ability a certain amount of time to move
public class ConditionUseActive : MoveCondition
{
    [SerializeField]
    private int requiredTimes;

    public override string Description()
    {
        return "Use Active " + requiredTimes + " Times";
    }


    public override void ResetProgress()
    {
        //Reset amount of times used
        moveProgress = 0f;
        UpdateCanMove();
    }

    //When totem's active is used, call this
    public void ActiveUsed()
    {
        //Increment progress
        moveProgress = Mathf.Min(moveProgress + 1 / (requiredTimes / 1.0f), 1.0f);
        UpdateCanMove();
    }
}
