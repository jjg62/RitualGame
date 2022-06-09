using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemies get movement behaviour from a class implementing this interface
public interface IEnemyMovement
{
    float Move();
}
