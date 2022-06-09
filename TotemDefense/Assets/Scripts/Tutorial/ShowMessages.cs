using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used by WaveSpawner at the start of each wave - if there are messages, displays them.
public class ShowMessages : MonoBehaviour
{
    public MessageInfo[] messages;

    public void Show(int wave)
    {
        foreach(MessageInfo m in messages)
        {
            //If it is the correct wave for this message, display it
            if(m.waveNumber == wave)
            {
                foreach(string text in m.messages)
                {
                    BattleUI.instance.ShowTutorialMessage(text);
                }
            }
        }
    }
}
