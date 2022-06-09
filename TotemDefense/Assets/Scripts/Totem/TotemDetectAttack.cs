using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adds behaviour to totem causing them to create a hitbox at intervals while enemy is in range
public class TotemDetectAttack : MonoBehaviour
{
    private Totem totem;

    private Animator anim;

    private TotemActive active;

    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private float range;

    private bool inRange;
    private bool attacking;

    [SerializeField]
    private Hitbox hitbox;

    [SerializeField]
    private float windUp;

    [SerializeField]
    private float coolDown;

    [SerializeField]
    private float offset;

    [SerializeField]
    private float hbHeight;

    [SerializeField]
    private float hbWidth;

    private void Start()
    {
        //Get references
        totem = GetComponent<Totem>();
        anim = GetComponent<Animator>();
        active = GetComponent<TotemActive>();
    }

    private void Update()
    {
        //Check if enemies are in range
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, totem.GetDirection() * Vector2.right, range, enemyLayer);
        Debug.DrawRay(transform.position, totem.GetDirection() * Vector2.right * range);
        inRange = raycast.collider != null;
        bool doingActive = active != null && active.inProgress;

        //If enemy in range and totem is not mid-ability
        if(inRange && !attacking && !doingActive)
        {
            //Start an attack
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        anim.SetTrigger("Attacking"); //Trigger aniamtaion

        //Wind up - time until attack lands
        yield return new WaitForSeconds(windUp);

        //Create the hitbox
        Hitbox hb = Instantiate(hitbox);
        hb.sourceTotem = totem;
        hb.transform.position = new Vector2(transform.position.x + totem.GetDirection() * offset, transform.position.y);
        hb.transform.localScale = new Vector2(hbWidth, hbHeight);

        //Cool down - time until attack animation ends after attack
        yield return new WaitForSeconds(coolDown);

        attacking = false;

    }

    //Stop any attacks the totem is in the process of doing
    public void AttackCancel()
    {
        attacking = false;
        StopAllCoroutines();
    }
}
