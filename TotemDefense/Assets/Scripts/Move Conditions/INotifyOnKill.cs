using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Totem scripts can implement this to be notified whenever they kill an enemy
public interface INotifyOnKill
{
    void OnKill(Enemy e);
}
