using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This active heals all above totems for a given amount
public class TotemActiveHealAbove : TotemActive
{
    //References
    private Totem totem;
    private Animator anim;
    private TotemDetectAttack attack;
    private ConditionUseActive condition;

    public float duration; //Duration of animation

    private void Start()
    {
        totem = GetComponent<Totem>();
        anim = GetComponent<Animator>();
        attack = GetComponent<TotemDetectAttack>();
        condition = GetComponent<ConditionUseActive>();
    }

    public override void Active()
    {
        inProgress = true;
        anim.SetTrigger("Active"); //Trigger animation
        attack.AttackCancel(); //Cancel attack if mid-attack

        Game.instance.sound.Play("Healer-Bark"); //Sound effect


        //Heal all above totems
        int pos = totem.stack.GetPosition(totem);

        for(int i = pos+1; i < totem.stack.GetHeight(); i++)
        {
            totem.stack.GetTotem(i).Heal(8);
        }

        if (condition != null) condition.ActiveUsed();

        //After the duration, set inProgress back to false
        Invoke("FinishActive", duration);
    }

    private void FinishActive()
    {
        inProgress = false;
    }

}
