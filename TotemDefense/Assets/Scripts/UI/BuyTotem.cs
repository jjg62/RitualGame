using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

//UI Object allowing player to hold left click and drag to build totems
public class BuyTotem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Totem totemPrefab;
    private Sprite dragSprite;

    [SerializeField]
    private int price;
    private bool doublePrice;
    [SerializeField]
    private TextMeshProUGUI priceLabel;

    private const float DROP_RADIUS = 1.75f;

    private TotemStack targetStack;

    [SerializeField]
    private DragDropIndicator indicatorPref;
    private DragDropIndicator indicator;
    private bool held;
    
    private bool showMore;
    [SerializeField]
    private GameObject moreDetails;

    [SerializeField]
    private TextMeshProUGUI notEnough;

    private void Start()
    {
        held = false;
        dragSprite = totemPrefab.GetComponent<SpriteRenderer>().sprite;
        moreDetails.SetActive(false);
        priceLabel.text = price.ToString();
        DoublePrice(true);
    }

    //When mouse is hovering over
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Show details of totem
        showMore = true;
        ShowMoreDetails();
    }

    //When mouse leaves
    public void OnPointerExit(PointerEventData eventData)
    {
        //Hide details
        showMore = false;
        ShowMoreDetails();
    }

    //When mouse is clicked
    public void OnPointerDown(PointerEventData eventData)
    {
        int finalPrice = price * (doublePrice ? 2 : 1); //Account for double price
        if (Game.instance.CanAfford(finalPrice))
        {
            //Create indicator and move it to mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            indicator = Instantiate(indicatorPref);
            indicator.SetSprite(dragSprite);
            indicator.transform.position = Camera.main.ScreenToWorldPoint(transform.position);
            held = true;

            //Slow time
            Time.timeScale = 0.3f;

        }
        else
        {
            //Alert player that they don't have enough
            notEnough.alpha = 1f;
            Game.instance.sound.Play("Wrong");
        }
    }

    //When mouse released
    public void OnPointerUp(PointerEventData eventData)
    {
        int finalPrice = price * (doublePrice ? 2 : 1);
        if (held)
        {
            held = false;
            indicator.End();

            //If near the top of a stack, and player can place a totem
            if (targetStack != null && Game.instance.CanAfford(finalPrice) && targetStack.CanPlace(false))
            {
                //Create the totem
                targetStack.AddTotem(totemPrefab);
                Game.instance.AddSouls(-finalPrice);

                //Set ability to ready
                ActiveMeter ac = targetStack.GetTopTotem().gameObject.GetComponentInChildren<ActiveMeter>();
                if (ac != null)
                {
                    ac.MakeReady();
                }
            }
            Time.timeScale = 1f;
            ShowMoreDetails();
        } 
    }

    private void Update()
    {
        if (held) //If player is dragging a totem from this buy
        {
            TotemStack rTotemStack = Game.instance.rightTotemStack;
            TotemStack lTotemStack = Game.instance.leftTotemStack;

            //Get distance from mouse to top of both totems
            float dr = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), rTotemStack.GetTop());
            float dl = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), lTotemStack.GetTop());

            //Snap indicator to target stack if distance is small enough
            if (dr < DROP_RADIUS)
            {
                if (targetStack != rTotemStack) indicator.SnapTo(rTotemStack.GetTop());
                targetStack = rTotemStack;
            }
            else if (dl < DROP_RADIUS)
            {
                if (targetStack != lTotemStack) indicator.SnapTo(lTotemStack.GetTop());
                targetStack = lTotemStack;
            }
            else
            {
                if (targetStack != null) indicator.UnSnap();
                targetStack = null;
            }

            //Check if totem can be placed
            indicator.SetCanPlace(targetStack == null || targetStack.CanPlace(false));
        }
        
        //If warning has been triggered, fade it out
        if(notEnough.alpha > 0f)
        {
            notEnough.alpha = Mathf.Max(0, notEnough.alpha - Time.deltaTime);
        }
    }

    //Toggle details, depending on if mouse is hovered over buy or player is dragging out totem
    private void ShowMoreDetails()
    {
        moreDetails.SetActive(showMore || held);
    }

    public void DoublePrice(bool dub)
    {
        doublePrice = dub;
        priceLabel.text = (price *  (dub ? 2 : 1)).ToString();
    }
}
