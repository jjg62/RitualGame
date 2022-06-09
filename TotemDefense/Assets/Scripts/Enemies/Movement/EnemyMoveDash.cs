using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When enemy is in range of totem, start charging a quick dash to close the gap
public class EnemyMoveDash : MonoBehaviour, IEnemyMovement
{
    private Animator anim;

    [SerializeField]
    private float normalSpeed; //Default move speed

    [SerializeField]
    private float dashSpeed; //Speed while dashing

    [SerializeField]
    private float dashRange;

    private bool dashUsed;

    private void Awake()
    {
        speed = normalSpeed;
        anim = GetComponent<Animator>();
    }

    private float speed;
    //Return current move speed
    public float Move()
    {
        if (!dashUsed && Mathf.Abs(transform.position.x) < dashRange)
        {
            dashUsed = true;
            StartCoroutine(Dash());
        }
        return speed;
    }

    //Back up slowly then dash forwards after a delay
    IEnumerator Dash()
    {
        speed = -1f;
        anim.SetBool("Dashing", true);
        yield return new WaitForSeconds(1.5f);
        speed = dashSpeed;
    }
}
