using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Indicator which appears when player drags totem from shop, or moves one
public class DragDropIndicator : MonoBehaviour
{
    private SpriteRenderer spr;
    private Vector2 targetPos;
    private bool snapped;
    private bool canPlace;

    [SerializeField]
    private Color defaultColour;
    [SerializeField]
    private Color blockedColour;
    [SerializeField]
    private Color ritualColour;

    [SerializeField]
    private TextMeshPro reason;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!snapped)
        {
            //If the indicator is not snapped to the top of a stack, follow the mouse
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        //Unless game is paused (e.g. from message)
        if (Time.timeScale != 0) transform.position = Vector2.Lerp(transform.position, targetPos, (Time.deltaTime*8)/Time.timeScale);
        spr.flipX = transform.position.x < 0;
    }

    public void SetSprite(Sprite sprite)
    {
        spr.sprite = sprite;
    }

    //Fix position of indicator
    public void SnapTo(Vector2 pos)
    {
        snapped = true;
        targetPos = pos;
    }

    public void UnSnap()
    {
        snapped = false;
    }

    public void End()
    {
        Destroy(gameObject);
    }

    //Set the canPlace variable and change colour when it is changed for the first time
    public void SetCanPlace(bool canPlace)
    {
        if(this.canPlace != canPlace)
        {
            this.canPlace = canPlace;
            spr.color = canPlace ? defaultColour : blockedColour;
        }
    }

    //Set canPlace variable depending on whether stack is blocked, and whether ritual is complete
    public void SetCanPlace(bool canPlace, bool needRitual)
    {
        if (this.canPlace != canPlace)
        {
            this.canPlace = canPlace;
            spr.color = canPlace ? defaultColour : (needRitual ? ritualColour : blockedColour);
            reason.gameObject.SetActive(!canPlace && needRitual);
        }
    }


    public bool CanPlace()
    {
        return canPlace;
    }

}
