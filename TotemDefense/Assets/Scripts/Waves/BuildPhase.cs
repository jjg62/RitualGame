using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Encapsulates info about a build phase
[System.Serializable]
public class BuildPhase
{
    public int beforeWave; //Which wave it comes before

    //What to display in warning clouds
    public Enemy[] leftWarning;
    public Enemy[] rightWarning;
}
