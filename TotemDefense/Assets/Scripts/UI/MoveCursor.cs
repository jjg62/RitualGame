using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Cursor which is activated when moving totems
public class MoveCursor : MonoBehaviour
{
    [SerializeField]
    private Image moveCooldownBar;

    [SerializeField]
    private Texture openHand;

    [SerializeField]
    private Texture closedHand;

    private RawImage img;

    private void Start()
    {
        img = GetComponent<RawImage>();
    }

    //Change sprite depending on whether totem is cucrrently being moved
    public void SetMoving(bool moving)
    {
        img.texture = moving ? closedHand : openHand;
    }

    //Display cooldown as a meter around cursor
    public void SetMoveCooldown(float val) {
        moveCooldownBar.fillAmount = val;

        if(val > 0)
        {
            //While on cooldown, cursor has reduced opacity
            Color c = Color.white;
            c.a = 0.5f;
            img.color = c;
        }
        else
        {
            img.color = Color.white;
        }
    }
}
