using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Totems which can be placed onto stacks by player
public class Totem : MonoBehaviour, IHitable
{

    //References
    private TotemDetectAttack totemBehaviour;
    [HideInInspector]
    public MoveCondition moveCondition;
    private Animator anim;
    private SpriteRenderer spr;

    private int direction;
    [HideInInspector]
    public TotemStack stack;

    public string name;

    public bool active = false;

    public int maxHealth = 10;
    public int startingHealth;
    private int health;

    [SerializeField]
    private ParticleSystem healEffect;

    private void Awake()
    {
        //Get references
        totemBehaviour = GetComponent<TotemDetectAttack>();
        moveCondition = GetComponent<MoveCondition>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        SetHealth(startingHealth > 0 ? startingHealth : maxHealth);
    }

    private void Start()
    {
        //Get direction based on position in level
        direction = (int)Mathf.Sign(transform.position.x);
        transform.localScale = new Vector2(direction, 1f);
    }

    public int GetDirection()
    {
        return direction;
    }

    //Implementation of IHitable
    public void Hit(Hitbox hb)
    {
        //Reduce health and play sound
        SetHealth(health - hb.damage);
        Game.instance.sound.Play("bonk");
        if(health <= 0)
        {
            //Destroy totem when it is lowered below 0 hp
            RemoveTotem();
        }
        
    }

    //Safely destroy totem
    public void RemoveTotem()
    {
        GetComponent<TotemDragDrop>().CancelHold(); //Prevent totem from being moved if it sis currently held
        stack.DeleteTotem(this); //Remove from the stack
        Destroy(gameObject);
    }

    public void SetActive(bool active)
    {
        this.active = active;
        totemBehaviour.enabled = active;
    }

    //Set health, change sprite based on current health and update panel
    public void SetHealth(int amount)
    {     
        health = Mathf.Min(amount, maxHealth);
        anim.SetFloat("Health", health / (maxHealth / 1.0f));
        UpdateInfoPanel();
    }

    public int GetHealth()
    {
        return health;
    }

    public void UpdateInfoPanel()
    {
        if (BattleUI.instance.panel.activeTotem == this)
        {
            BattleUI.instance.panel.UpdateInfo(health / (maxHealth/1.0f), moveCondition.moveProgress);
        }
    }

    
    private IEnumerator healAnim;
    public void Heal(int amount)
    {
        //Start a healing animation (and cancel current one if still playing)
        SetHealth(health + amount);
        if(healAnim != null) StopCoroutine(healAnim);
        healAnim = HealAnimation();
        StartCoroutine(healAnim);
    }

    IEnumerator HealAnimation()
    {
        float t = 0;
        const float UP_TIME = 0.05f; //Time taken to climb to healed (green) colour
        const float DOWN_TIME = 0.5f; //Time taken to return to normal colour

        Color oldColour = Color.white;
        Color newColour = Color.green;

        //Gradually change to green
        while(t < UP_TIME)
        {
            t += Time.deltaTime;
            spr.color = Color.Lerp(oldColour, newColour, t / UP_TIME);
            yield return null;
        }

        //Create particles
        Instantiate(healEffect).transform.position = transform.position;

        //Gradually return to normal
        while (t < UP_TIME + DOWN_TIME)
        {
            t += Time.deltaTime;
            spr.color = Color.Lerp(newColour, oldColour, (t - UP_TIME) / DOWN_TIME);
            yield return null;
        }
    }

}
