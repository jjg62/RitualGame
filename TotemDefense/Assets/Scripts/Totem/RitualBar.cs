using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface meter which temporarily appears when totem makes progress towards ritual
public class RitualBar : MonoBehaviour
{

    [SerializeField]
    private Transform fill;

    private float target;

    private void Start()
    {
        ChangeProgress(0f);
        gameObject.SetActive(false);
    }

    //When progress is made, show an animation
    public void ShowAnimation(float target)
    {
        gameObject.SetActive(true);
        ChangeProgress(this.target); //Make sure bar has reached the old target
        this.target = target; //Change target
        StopCoroutine(ProgressAnimation());
        StartCoroutine(ProgressAnimation()); //Restart coroutine
    }

    //Gradually change meter value to target
    IEnumerator ProgressAnimation()
    {
        while(Mathf.Abs(target - fill.localScale.x) > 0.02f)
        {
            ChangeProgress(Mathf.Lerp(fill.localScale.x, target, Time.deltaTime*6));
            yield return null;
        }
        ChangeProgress(target);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    //Change progress of meter
    private void ChangeProgress(float prog)
    {
        Vector2 scale = fill.localScale;
        scale.x = prog;
        fill.localScale = scale;
    }
}
