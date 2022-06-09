using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Totem scripts can implement this to be notified whenever they hit an enemy
public interface INotifyOnHit
{
    void OnHit(Enemy e);
}
