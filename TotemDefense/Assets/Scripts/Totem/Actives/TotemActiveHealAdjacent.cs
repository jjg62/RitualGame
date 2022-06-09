using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This active heals adjacent totems (directly above and below) for a given amount
public class TotemActiveHealAdjacent : TotemActive
{
    //References
    private Totem totem;
    private Animator anim;
    private TotemDetectAttack attack;
    private ConditionUseActive condition;

    [SerializeField]
    private int healAmount; //Amount of HP to heal

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
        attack.AttackCancel(); //Cancel if mid-attack

        Game.instance.sound.Play("Healer-Bark"); //Sound effect

        //Heal totems above and below
        int pos = totem.stack.GetPosition(totem);

        Totem above = totem.stack.GetTotem(pos + 1);
        Totem below = totem.stack.GetTotem(pos - 1);

        if (above != null) above.Heal(healAmount);
        if (below != null) below.Heal(healAmount);

        if (condition != null) condition.ActiveUsed();

        Invoke("FinishActive", duration); //After duration, reset inProgress to false
    }

    private void FinishActive()
    {
        inProgress = false;
    }
    
}
