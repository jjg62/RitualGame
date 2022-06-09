using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Totem needs to land certain amount of finishing blows to move
public class ConditionGetKills : MoveCondition, INotifyOnKill
{
    [SerializeField]
    private int requiredKills;

    //Whenever this totem kills an enemy
    public void OnKill(Enemy e)
    {
        //Increment progress
        moveProgress = Mathf.Min(moveProgress + (1 / (requiredKills / 1.0f)), 1.0f);
        UpdateCanMove();
    }

    public override void ResetProgress()
    {
        //Reset kills to 0
        moveProgress = 0f;
        UpdateCanMove();
    }

    public override string Description()
    {
        return "Defeat " + requiredKills + " Enemies";
    }
}
