using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls zoom
public class CameraEffects : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private float defaultZoom = 5.0f;
    [SerializeField]
    private float slowdownZoom = 4.5f;

    private float targetZoom;

    private void Start()
    {
        cam = GetComponent<Camera>();
        targetZoom = defaultZoom;
    }

    //When time is slowed, zoom camera in
    private void Update()
    {
        targetZoom = Time.timeScale < 1.0f ? slowdownZoom : defaultZoom;
        if (Time.timeScale != 0) cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime*4 / Time.timeScale);

        //transform.position = new Vector3(transform.position.x + Input.GetAxisRaw("Horizontal") * 0.02f, 1, -10); //Debug controls
    }


}
