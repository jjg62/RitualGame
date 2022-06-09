using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Attached to message prefab, created when message screen is shown
public class TutorialMessage : MonoBehaviour
{
    public TextMeshProUGUI message; //Textbox containing message
    private float oldTime; //Timescale before message is displayed
    public TutorialMessage nextMessage; //Next message in list

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        //Stop time and show message
        gameObject.SetActive(true);
        oldTime = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Return()
    {
        //Resume time and destroy message
        Time.timeScale = oldTime;
        Destroy(gameObject);

        //Show next message
        if (nextMessage != null) nextMessage.Show();
        BattleUI.instance.msgHead = nextMessage; 
    }
}
