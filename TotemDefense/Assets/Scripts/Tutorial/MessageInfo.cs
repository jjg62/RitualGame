using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Encapsulates one tutorial message screen as shown in inspector
[System.Serializable]
public class MessageInfo
{
    public int waveNumber; //Which wave to display the message screen
    public string[] messages; //All consecutive messages shown
}
