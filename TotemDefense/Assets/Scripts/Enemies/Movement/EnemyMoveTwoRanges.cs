using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//While a totem is placed on the same side, stay within longRange and at least further away from shortRange. Otherwise, keep moving forward.
public class EnemyMoveTwoRanges : MonoBehaviour, IEnemyMovement
{
    private Enemy enemy;
    private SpriteRenderer spr;

    [SerializeField]
    private float moveSpeed; //Speed to move forwards
    [SerializeField]
    private float backpedalSpeed; //Speed to move backwards
    [SerializeField]
    private LayerMask totemLayer;
    [SerializeField]
    private float longRange; //Move forward until within this range
    [SerializeField]
    private float shortRange; //Move backwards until within this range

    [SerializeField]
    private float randomDistanceAdjustment; //Randomly adjust distances slightly such that enemies don't stack directly on top of each other

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        spr = GetComponent<SpriteRenderer>();

        longRange -= Random.Range(0, randomDistanceAdjustment);
        shortRange += Random.Range(0, randomDistanceAdjustment);
    }

    //Return current move speed
    public float Move()
    {

        //Check whether a totem is in longRange and shortRange resp.
        RaycastHit2D tooFar = Physics2D.Raycast(transform.position, enemy.GetDirection() * Vector2.right, longRange, totemLayer);
        RaycastHit2D tooClose = Physics2D.Raycast(transform.position, enemy.GetDirection() * Vector2.right, shortRange, totemLayer);

        if (tooClose.collider != null && SameSide(tooClose.collider.gameObject)) //Need to check enemy is not coming up behind other side
        {
            //Too close, move backwards
            spr.flipX = !spr.flipX;
            return -backpedalSpeed;
        }
        else if (tooFar.collider != null && SameSide(tooFar.collider.gameObject))
        {
            //Totem in longRange, stop moving
            return 0;
        }
        else
        {
            //Need to get closer
            return moveSpeed;
        }
    }

    //Is this object on the same side of the level as g?
    private bool SameSide(GameObject g)
    {
        return g.transform.position.x * transform.position.x >= 0;
    }
}
