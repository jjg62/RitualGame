using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Periodically creates hitbox when in range
public class EnemyAttack : MonoBehaviour, IHitstunListener
{
    private Enemy enemy;
    private Rigidbody2D rb;
    private Animator anim;

    private EnemyAttackChange attackChange;

    [SerializeField]
    private float range;
    [SerializeField]
    private float offsetX;

    [SerializeField]
    private float offsetY;

    [SerializeField]
    private float hbHeight;
    [SerializeField]
    private float hbWidth;

    [SerializeField]
    private LayerMask totemLayer;

    private bool activated;
    private bool attacking;

    [SerializeField]
    private float windUp = 0.2f;
    [SerializeField]
    private float coolDown = 0.2f;

    [SerializeField]
    private bool attackOnlyWhenStill = false;

    [SerializeField]
    private Hitbox hitbox;


    public void Hitstun(Hitbox hb)
    {
        //Cancel attack if hit by a totem
        if(hb.hitstunDuration > 0)
        {
            StopAllCoroutines();
            attacking = false;
        }   
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        enemy.AddListener(this);
        anim = GetComponent<Animator>();

        attackChange = GetComponent<EnemyAttackChange>();
    }

    private void Update()
    {
        //Check if totems in range
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, enemy.GetDirection() * Vector2.right, range, totemLayer);
        Debug.DrawRay(transform.position, enemy.GetDirection() * Vector2.right * range);
        activated = raycast.collider != null && SameSide(raycast.collider.gameObject); //Check totems are on the right side

        bool attackReady = !attackOnlyWhenStill || rb.velocity.magnitude == 0;

        if (activated && !attacking && !enemy.hitstun && attackReady)
        {
            StartCoroutine(Attack());
        }

        //Notify animator
        anim.SetBool("Attacking", attacking);
    }

    IEnumerator Attack()
    {
        attacking = true;

        yield return new WaitForSeconds(windUp); //windUp denoted time before attack lands
        Hitbox hb = Instantiate(hitbox); //Create the hitbox
        hb.transform.position = new Vector2(transform.position.x + enemy.GetDirection() * offsetX, transform.position.y + offsetY);
        hb.transform.localScale = new Vector2(hbWidth, hbHeight);


        yield return new WaitForSeconds(coolDown);
        if (attackChange != null) attackChange.Attacked(); //If this enemy changes after a certain amount of attacks, take note of this attacks
        attacking = false;
    }

    private bool SameSide(GameObject g)
    {
        return g.transform.position.x * transform.position.x >= 0;
    }

}
