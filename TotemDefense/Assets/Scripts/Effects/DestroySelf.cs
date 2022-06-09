using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Destroy this object after a given amount of time
public class DestroySelf : MonoBehaviour
{
    [SerializeField]
    private float after;

    private void Start()
    {
        Destroy(gameObject, after);
    }
}
