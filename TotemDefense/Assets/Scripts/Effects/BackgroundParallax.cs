using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parallax effect for background sprites
public class BackgroundParallax : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float depth;


    //Depending on depth of object in background, change position and sixe when camera moves/zooms
    private void Update()
    {
        //Zoom
        float newSize = Mathf.Lerp(1f, Camera.main.orthographicSize / 5.0f, depth);
        transform.localScale = new Vector2(newSize, newSize);

        //Movement
        float newX = Mathf.Lerp(0, Camera.main.transform.position.x, depth);
        transform.position = new Vector2(newX, transform.position.y);
    }
}
