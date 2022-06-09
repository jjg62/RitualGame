using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Movement of a "sniper"/hitscan bullet which hits top totem
public class SnipeMovement : MonoBehaviour
{

    private void Start()
    {
        TotemStack leftStack = GameObject.FindWithTag("Left Stack").GetComponent<TotemStack>();
        TotemStack rightStack = GameObject.FindWithTag("Right Stack").GetComponent<TotemStack>();

        //Instanstly move to top of totem
        Vector3 newPos = (transform.position.x > 0 ? rightStack : leftStack).GetTopTotem().transform.position;

        //Angle the object such that it points from created position to top totem
        Vector3 distance = newPos - transform.position;
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.position = newPos;
        
    }
}
