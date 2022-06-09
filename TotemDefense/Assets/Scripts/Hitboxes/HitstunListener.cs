using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for enemy scripts that are notified when the enemy has been hit
public interface IHitstunListener
{
    void Hitstun(Hitbox hb);
}
