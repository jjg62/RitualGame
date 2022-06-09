using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Visual and sound effect when totems hit the ground
public class GroundHitEffect : MonoBehaviour
{
    private BoxCollider2D col;
    private Rigidbody2D rb;

    private bool grounded;
    private bool effectPlayed;
    private const float SPEED_THRESHOLD = 1f;

    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField]
    private GameObject dust;


    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        effectPlayed = false;
    }

    private void Update()
    {
        //Check if totem is on the ground
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position - new Vector3(0, col.bounds.extents.y + 0.05f, 0), Vector2.down, 0.1f, groundLayerMask);
        grounded = groundCheck.collider != null;
        //Debug.DrawRay(transform.position - new Vector3(0, col.bounds.extents.y + 0.05f, 0), Vector2.down * 0.1f, grounded ? Color.green : Color.red);

        //If they are and weren't previously
        if (grounded && !effectPlayed && rb.velocity.y < -1 * SPEED_THRESHOLD)
        {
            //Play effect once 
            effectPlayed = true;
            Game.instance.sound.Play("GroundHit");
            Instantiate(dust, transform);
        }

    }
}
