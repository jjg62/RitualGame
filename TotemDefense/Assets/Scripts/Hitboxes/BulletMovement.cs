using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Makes bullet hitboxes move
public class BulletMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float speed;

    private float startX;

    private void Start()
    {
        //Set to start position
        startX = transform.position.x;

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(Mathf.Sign(transform.position.x) * speed, 0); //Set to a fixed velocity
    }

    private void Update()
    {
        //When bullet changes side, delete it
        if (transform.position.x * startX <= 0) Destroy(gameObject);
    }

}
