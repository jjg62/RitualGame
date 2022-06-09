using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Allow totems to be moved by dragging and dropping
public class TotemDragDrop : MonoBehaviour
{
    private Totem totem;
    private TotemActive active;
    private MoveCondition moveCondition;
    private SpriteRenderer spr;
    private const float DROP_RADIUS = 1.75f;

    private TotemStack targetStack;

    [SerializeField]
    private DragDropIndicator indicatorPref;
    private DragDropIndicator indicator;
    private bool held;


    private void Start()
    {
        //Get references
        totem = GetComponent<Totem>();
        active = GetComponent<TotemActive>();
        moveCondition = GetComponent<MoveCondition>();
        spr = GetComponent<SpriteRenderer>();
        held = false;
    }


    //When mouse first starts to hover over totem
    private void OnMouseEnter()
    {
        //Update the info panel
        BattleUI.instance.panel.SetActiveTotem(totem, moveCondition.Description());
        totem.UpdateInfoPanel();

        //Change cursor sprite
        BattleUI.instance.cursor.hovering = true;
        BattleUI.instance.cursor.ChangeCursor(1);
    }

    //When mosue leaves totem
    private void OnMouseExit()
    {
        //Hide info panel
        BattleUI.instance.panel.Disable();

        //Change cursor sprite
        BattleUI.instance.cursor.hovering = false;
        if (!BattleUI.instance.cursor.IsMoving())
        {
            BattleUI.instance.cursor.ChangeCursor(0);
        }
    }

    //When mouse is clicked down
    private void OnMouseDown()
    {
        //If can move totem and not mid-active
        if (Game.instance.canMove && Time.timeScale > 0f)
        {
            if (active == null || !active.inProgress)
            {
                //Create indicator at mouse position
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                indicator = Instantiate(indicatorPref);
                indicator.SetSprite(spr.sprite);
                indicator.transform.position = transform.position;
                held = true;
                BattleUI.instance.cursor.SetMoving(this);
                Time.timeScale = 0.3f; //Slow down time
            }
            else
            {
                Game.instance.sound.Play("Wrong");
            }

        }
    }

    private void Update()
    {
        if (held)
        {
            //While mosue button is held on a totem, track distance to either totem stack
            TotemStack rTotemStack = Game.instance.rightTotemStack;
            TotemStack lTotemStack = Game.instance.leftTotemStack;

            TotemStack currentStack = totem.stack; 

            float dr = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), rTotemStack.GetTop());
            float dl = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), lTotemStack.GetTop());

            //Snap to top of totem stack if within range
            if (dr < DROP_RADIUS && rTotemStack.GetTopTotem() != totem)
            {
                if (targetStack != rTotemStack) indicator.SnapTo(rTotemStack.GetTop());
                targetStack = rTotemStack;
            }
            else if (dl < DROP_RADIUS && lTotemStack.GetTopTotem() != totem)
            {
                if (targetStack != lTotemStack) indicator.SnapTo(lTotemStack.GetTop());
                targetStack = lTotemStack;
            }
            else
            {
                if (targetStack != null) indicator.UnSnap();
                targetStack = null;
            }

            //Is the totem allowed to be moved to this position (if it's the other stack, need to have completed ritual)
            bool ritualOK = (targetStack == currentStack || moveCondition.moveProgress >= 1f);

            indicator.SetCanPlace(targetStack == null || (targetStack.CanPlace(targetStack == currentStack) && ritualOK), !ritualOK);
        }
    }

    //When mouse button released
    private void OnMouseUp()
    {
        if (held)
        {
            held = false;
            BattleUI.instance.cursor.SetMoving(null);
            indicator.End();
            Time.timeScale = 1.0f; //Resume timescale

            //Change cursor sprite
            if(!BattleUI.instance.cursor.hovering) BattleUI.instance.cursor.ChangeCursor(0);

            //Place totem on new stack
            if (targetStack != null && indicator.CanPlace())
            {
                if(targetStack != totem.stack) totem.moveCondition.ResetProgress();
                targetStack.AddTotem(totem);

                Game.instance.TotemMoved();

                totem.RemoveTotem(); //Remove from current stack
            }
            else if (!indicator.CanPlace())
            {
                Game.instance.sound.Play("Wrong");
            }

        }
    }

    //If mouse is held (and e.g. totem dies) cancel the move
    public void CancelHold()
    {
        if (held)
        {
            held = false;
            BattleUI.instance.cursor.SetMoving(null);
            if (!BattleUI.instance.cursor.hovering) BattleUI.instance.cursor.ChangeCursor(0);
            indicator.End();
            Time.timeScale = 1.0f;
        }
    }
}
