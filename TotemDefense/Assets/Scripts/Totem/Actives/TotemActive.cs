using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ability used by totems when player gives command
public abstract class TotemActive : MonoBehaviour
{
    public float cooldown; //Time taken to charge active
    public bool inProgress; //Whether the active is being used
    public abstract void Active(); //Effect of active
}
