using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An enemy which attacks totems and can be attacked
public class Enemy : MonoBehaviour, IHitable
{
    private SpriteRenderer spr;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Animator anim;
    private IEnemyMovement movement;

    private int direction;

    public int health;
    public bool hitstun;
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Color normalColor;

    private List<IHitstunListener> listeners;

    [HideInInspector]
    public Wave wave;

    [SerializeField]
    private GameObject deathAnimation;

    private void Awake()
    {
        listeners = new List<IHitstunListener>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        movement = GetComponent<IEnemyMovement>();
        hitstun = false; 
    }

    private void Update()
    {
        //Get direction based on position in the level
        direction = (int)Mathf.Sign(Game.instance.centre.transform.position.x - transform.position.x);
        spr.flipX = direction > 0;

        //Tell animator when enemy is in hitstun
        anim.SetBool("Hitstun", hitstun);

        //Get movement from IEnemyMovement implementation
        if (!hitstun) rb.velocity = new Vector2(direction * movement.Move(), rb.velocity.y);
    }

    //Whenever enemy is hit
    public void Hit(Hitbox hb)
    {
        if (!hitstun || hb.transcendHitstun) //Some special hitboxes still hit when an enemy is stunned from previous hit
        {
            StartCoroutine(HitEffect(hb.hitstunDuration, hb.knockbackDistance + Random.Range(0, 0.5f))); //Vary hitstun distance randomly to prevent enemies stacking

            foreach (IHitstunListener listener in listeners)
            {
                //Notify listeners when this enemy is hit
                listener.Hitstun(hb);
            }

            //Notify move condition that a hit occured
            if (hb.sourceTotem != null && hb.sourceTotem.moveCondition is INotifyOnHit)
            {
                ((INotifyOnHit)hb.sourceTotem.moveCondition).OnHit(this);
            }

            health-= hb.damage;

            //Death
            if (health <= 0)
            {

                if(hb.sourceTotem != null && hb.sourceTotem.moveCondition is INotifyOnKill)
                {
                    ((INotifyOnKill)hb.sourceTotem.moveCondition).OnKill(this); //If killed by a totem, notify it
                }
                wave.EnemyKilled();
                Destroy(gameObject);

                if (deathAnimation != null)
                {
                    Transform t = Instantiate(deathAnimation).transform;
                    t.position = transform.position;
                    t.localScale = new Vector2(Mathf.Sign(transform.position.x), 1);
                }
            }

            hb.PlayHitSound();
        }
    }

    private IEnumerator redFlash;
    IEnumerator HitEffect(float hitstunDuration, float knockbackDistance)
    {
        hitstun = true;

        float t = 0f;
        float end = hitstunDuration;

        float kSpeed = (-1 * direction) * 2 * knockbackDistance / hitstunDuration; //Correct initial speed such that enemy is knocked back the right distance

        //Jump effect
        float jf = -0.5f * Physics2D.gravity.y * hitstunDuration; //Correct initial speed such that enemy is in the air while in hitstun
        rb.velocity = new Vector2(rb.velocity.x, jf);

        //Red flash effect
        if(redFlash != null) StopCoroutine(redFlash);
        redFlash = RedFlash();
        StartCoroutine(redFlash);

        while(t < end)
        {
            //Gradually decrease horizontal velocity
            t += Time.deltaTime;

            float xSpeed = Mathf.Lerp(kSpeed, 0, t/end);
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);

            yield return null;
        }
        

        hitstun = false;
    }

    IEnumerator RedFlash()
    {
        spr.color = Color.red;
        float t = 0f;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            spr.color = Color.Lerp(Color.red, normalColor, t / 1.0f);
            yield return null;
        }
        spr.color = normalColor;

    }

    public int GetDirection()
    {
        return direction;
    }

    public void AddListener(IHitstunListener listener)
    {
        listeners.Add(listener);
    }

}
