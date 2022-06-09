using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The trigger in the middle of the level, which ends the game if enemies get near
//Or, if enabled, blasts enemies back the first time, giving players a second chance
public class GameOver : MonoBehaviour
{
    [SerializeField]
    private Hitbox instakillHitbox;
    [SerializeField]
    private Hitbox largeHitbox;
    [SerializeField]
    private GameObject effect;

    public int lives = 2;
    private bool vulnerable = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (vulnerable && collision.gameObject.GetComponent<Enemy>() != null){
            //Enemy has walked into range
            lives--;
            vulnerable = false; //Temporarily make the shrine invincible
            if(lives <= 0)
            {
                //Game over
                StartCoroutine(Game.instance.RestartGame());
            }
            else
            {
                //Blast enemies back and instantly kill close ones
                StartCoroutine(SafetyBlast());
            }
            
        }
    }

    IEnumerator SafetyBlast()
    {
        //Small hitbox that instantly kills enemies, large one that knocks back
        Instantiate(instakillHitbox).transform.position = transform.position;
        Instantiate(effect).transform.position = transform.position;
        Instantiate(largeHitbox).transform.position = transform.position;

        yield return new WaitForSeconds(2f);

        //If this happens for the first time, explain to the player 
        if (!LevelManager.instance.BlastTutorial)
        {
            LevelManager.instance.BlastTutorial = true;
            BattleUI.instance.ShowTutorialMessage("The first time it is attacked, the Shrine will unleash an Energy Blast, knocking back enemies and giving you a second chance.");
        }

        vulnerable = true;
    }
}
