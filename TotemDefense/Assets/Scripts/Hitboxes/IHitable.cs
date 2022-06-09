using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for entities that can be hit by hitboxes
public interface IHitable
{
    void Hit(Hitbox hb);
}
