using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Shows how close active is to being charged (and soul cost if there is one)
public class ActiveMeter : MonoBehaviour
{
    private SpriteRenderer spr;

    #region Meter
    [SerializeField]
    private SpriteMask mask;
    private bool fillEffect;

    [SerializeField]
    private TextMeshPro costText; //Textbox showing soul cost

    [SerializeField]
    private int cost = 0;

    //Gradually fill meter
    private void UpdateProgress(float progress)
    {
        //Fill effect has an alpha gradient - so use mask to control progress
        mask.alphaCutoff = 1 - (progress/active.cooldown);

        if(!fillEffect && progress >= active.cooldown)
        {
            //Play an effect when meter first fills
            fillEffect = true;
            spr.color = Color.white;
            Game.instance.sound.Play("Active-Bar-Full");

            if(cost > 0) costText.gameObject.SetActive(true);
        }
    }
    #endregion

    private float progress;
    [SerializeField]
    private TotemActive active;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.color = Color.gray;

        costText.gameObject.SetActive(false);
        costText.transform.localScale = new Vector2(Mathf.Sign(transform.position.x), 1);
        costText.text = cost.ToString();
    }

    //When user clicks on meter
    private void OnMouseDown()
    {
        //If meter is full
        if (progress >= active.cooldown)
        {
            if (Game.instance.CanAfford(cost)) //If player has enough souls
            {
                Game.instance.AddSouls(-cost); //Take them away

                active.Active(); //Activate the active ability

                //Reset progress
                progress = 0;
                UpdateProgress(progress);
                costText.gameObject.SetActive(false);

                fillEffect = false;
                spr.color = Color.gray;
            }
            else
            {
                //Bring attention to cost text
                textWarningTime = 1f;
                Game.instance.sound.Play("Wrong");
            }

            
        }
    }

    private float textWarningTime;

    private void Update()
    {
        //Gradually update progress
        if(!BattleUI.instance.buildPhase) progress = Mathf.Min(active.cooldown, progress + Time.deltaTime);
        UpdateProgress(progress);

        //If warning has been triggered, gradually change back
        if(textWarningTime > 0f)
        {
            textWarningTime = Mathf.Max(textWarningTime - Time.deltaTime, 0);
            costText.color = Color.Lerp(Color.magenta, Color.grey, textWarningTime);
        }
    }

    //Fill progress bar
    public void MakeReady()
    {
        progress = active.cooldown;
    }

    public void SetProgress(float prog)
    {
        this.progress = prog;
    }

    public float GetProgress()
    {
        return progress;
    }
}
