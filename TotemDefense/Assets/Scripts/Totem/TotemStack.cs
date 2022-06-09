using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Positions where totems can be placed on top of each other
public class TotemStack : MonoBehaviour
{
    private BoxCollider2D col;
    private CheckBlockingPlacement blockCheck; //Area to check whether enemies are in the way of totem placement

    private List<Totem> totems;

    [SerializeField]
    private int height;
    private int direction;

    private const float TOTEM_HEIGHT = 1f;
    private const float TOTEM_WIDTH = 1.5f;

    public int maxHeight = 4;


    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        blockCheck = GetComponentInChildren<CheckBlockingPlacement>();
        height = 0;
        totems = new List<Totem>();
        direction = (int)Mathf.Sign(transform.position.x);
    }

    private void Update()
    {
        //Change position and size of collider based on stack height
        col.offset = new Vector2(direction*(TOTEM_WIDTH/2f + 0.05f), height * TOTEM_HEIGHT/2f);
        col.size = new Vector2(0.02f, height * TOTEM_HEIGHT);
    }

    //Add a totem to top of stack
    public void AddTotem(Totem totem)
    {
        //Create the totem
        Totem newTotem = Instantiate(totem, transform);
        newTotem.stack = this;
        newTotem.transform.position = GetTop(); //Move to top

        //Preserve Health
        int hp = totem.GetHealth();
        if (hp == 0) hp = totem.startingHealth > 0 ? totem.startingHealth : totem.maxHealth; //If hp is 0, it's a new totem so set at starting health
        newTotem.SetHealth(hp);

        //Preserve active meter progress
        ActiveMeter ac = totem.GetComponentInChildren<ActiveMeter>();
        if(ac != null)
        {
            newTotem.GetComponentInChildren<ActiveMeter>().SetProgress(ac.GetProgress());
        }

        totems.Add(newTotem);
        height++;
        ReassignActive();

        Game.instance.sound.Play("TotemPlace");

    }

    //Remove totem from stack
    public void DeleteTotem(Totem totem)
    {
        if (totems.Remove(totem))
        {
            height--;
            ReassignActive();
        }
        
    }

    //Set only bottom totem to be active
    private void ReassignActive()
    {
        for (int i = 0; i < totems.Count; i++)
        {
            totems[i].SetActive(i == 0);
        }
    }

    //Get position of stop of stack
    public Vector2 GetTop()
    {
        return new Vector2(transform.position.x, transform.position.y + (height + 0.5f) * TOTEM_HEIGHT + 0.8f);
    }

    //Get top totem instance
    public Totem GetTopTotem()
    {
        if (totems.Count > 0)
        {
            return totems[totems.Count - 1];
        }
        else
        {
            return null;
        }
    }

    public int GetHeight()
    {
        return height;
    }

    //Check if totem can be placed on top of this stack
    public bool CanPlace(bool sameStack)
    {
        return !blockCheck.IsEnemyBlocking() && !Full(sameStack);
    }

    //Get a totem in a specific position
    public Totem GetTotem(int pos)
    {
        if(pos >= 0 && pos < totems.Count)
        {
            return totems[pos];
        }
        else
        {
            return null;
        }
    }

    public int GetPosition(Totem totem)
    {
        return totems.IndexOf(totem);
    }

    //See if stack is full of totems
    public bool Full(bool sameStack)
    {
        //If the totem being placed is already in the stack and being moved to top, give one more space of leeway because it'll be removed from bottom
        return height >= maxHeight + (sameStack ? 1 : 0);
    }
}
