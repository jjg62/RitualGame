using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A level, consisting of multiple waves of enemies
[System.Serializable]
public class Level
{
    public string levelName;
    public Wave[] waves;
    public MessageInfo[] messages;
    public BuyTotem[] buys; //Which totem buys are available

    public int startingSouls = 10;

    //Totems which are spawned in automatically at the beginning at the level
    public Totem[] leftStartingTotems;
    public Totem[] rightStartingTotems;

    public BuildPhase[] buildPhases;

    public string music;
}
