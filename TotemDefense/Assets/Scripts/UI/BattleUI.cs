using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//HUD elements for main game
public class BattleUI : MonoBehaviour
{
    //Singleton
    public static BattleUI instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        fade.gameObject.SetActive(true);
    }

    public RectTransform soulCounter;
    public InfoPanel panel;
    public RectTransform buildPhaseUI;

    [SerializeField]
    private RawImage fade;

    [SerializeField]
    private TutorialMessage tutorialMessagePrefab;

    [SerializeField]
    private Transform tutorialLayer;

    public TutorialMessage msgHead;

    [SerializeField]
    private BuyTotem[] anytimeBuys;

    [SerializeField]
    private Animator leftShrineCloud;

    [SerializeField]
    private Animator rightShrineCloud;

    public MainCursor cursor;

    public bool buildPhase;

    //Show UI elements for build phase
    public void ShowBuildPhase(bool show)
    {
        buildPhase = show;

        //Trigger an animation for shrine hint clouds
        string trigger = show ? "Start" : "End";
        leftShrineCloud.SetTrigger(trigger);
        rightShrineCloud.SetTrigger(trigger);
        

        buildPhaseUI.gameObject.SetActive(show);
    }

    public void ShowTutorialMessage(string msg)
    {
        //If player is dragging a totem, cancel that
        if (cursor.moving != null) cursor.moving.CancelHold();

        TutorialMessage newMsg = Instantiate(tutorialMessagePrefab, tutorialLayer); //Create message prefab
        newMsg.message.text = msg.Replace("\\n", "\n"); //Fix message newline characters
        if(msgHead != null)
        {
            //If there is already a queue of messages, append this one
            msgHead.nextMessage = newMsg;
        }
        else
        {
            //No queue, show this message
            newMsg.Show();
        }
        msgHead = newMsg;
    }

    //Fade to / out of black
    public IEnumerator FadeTo(Color c, float duration)
    {
        Color startColour = fade.color;
        float t = 0;
        while (t < duration)
        {
            fade.color = Color.Lerp(startColour, c, t/duration);
            t += Time.deltaTime;
            yield return null;
        }
        fade.color = c;

    }
}
