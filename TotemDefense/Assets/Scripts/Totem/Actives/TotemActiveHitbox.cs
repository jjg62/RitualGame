using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This active creates a hitbox
public class TotemActiveHitbox : TotemActive
{
    //References
    private Totem totem;
    private Animator anim;
    private TotemDetectAttack attack;
    
    [SerializeField]
    private Hitbox hitbox; //Hitbox to create

    [SerializeField]
    private float windUp; //Time before hitbox is created

    [SerializeField]
    private float coolDown; //Time after hitbox is created before Totem can resume attacking

    [SerializeField]
    private float xOffset; //X position of hitbox relative to totem

    [SerializeField]
    private float y; //Y position of hitbox
    [SerializeField]
    private bool relative; //Whether the y position should be relative to the totem, or the world


    [SerializeField]
    private GameObject effect; //Any visual effects

    private ConditionUseActive condition;

    private void Start()
    {
        totem = GetComponent<Totem>();
        anim = GetComponent<Animator>();
        attack = GetComponent<TotemDetectAttack>();
        condition = GetComponent<ConditionUseActive>();
    }

    public override void Active()
    {
        anim.SetTrigger("Active"); //Trigger animation
        attack.AttackCancel(); //If mid-attack, cancel it
        StartCoroutine(Attack()); 
        if (condition != null) condition.ActiveUsed(); //If totem has a Use Active move condition, notify it
    }

    //Spawn hitbox accounting for windup and cooldown
    IEnumerator Attack()
    {
        inProgress = true;

        //Create visual effects if there are any
        if (effect != null)
        {
            GameObject fx = Instantiate(effect);
            fx.transform.position = new Vector2(transform.position.x + totem.GetDirection() * xOffset, y);
            fx.GetComponent<SpriteRenderer>().flipX = transform.position.x < 0;

        }

        yield return new WaitForSeconds(windUp);

        //Create hitbox after windup
        Hitbox hb = Instantiate(hitbox);
        hb.sourceTotem = totem;
        hb.transform.position = new Vector2(transform.position.x + totem.GetDirection() * xOffset, (relative ? transform.position.y + y : y));
        hb.transform.localScale = new Vector2(Mathf.Sign(transform.position.x) * hb.transform.localScale.x, hb.transform.localScale.y);

        yield return new WaitForSeconds(coolDown);
        inProgress = false;
    }
}
