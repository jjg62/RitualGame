using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//General game manager object
public class Game : MonoBehaviour
{
    public static Game instance;

    //Singleton
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            sound = GetComponent<AudioManager>();
        }
    }

    [HideInInspector]
    public AudioManager sound;

    public GameOver centre;
    public TotemStack rightTotemStack;
    public TotemStack leftTotemStack;
    [SerializeField]
    private WaveSpawner spawner;

    private int souls;
    [SerializeField]
    private TextMeshProUGUI soulCount;

    [HideInInspector]
    public bool canMove;

    private float moveTimer = 0f;
    private const float MOVE_COOLDOWN = 2.5f;

    //Give the player souls (currency)
    public void AddSouls(int amount)
    {
        souls += amount;
        soulCount.text = souls.ToString();
    }

    //Check if player can afford something
    public bool CanAfford(int amount)
    {
        return souls >= amount;
    }

    private void Start()
    {
        Color c = Color.black;
        c.a = 0;

        //At the start of each level, fade from black to transparent
        StartCoroutine(BattleUI.instance.FadeTo(c, 1.5f));

        //Set up the level
        LevelManager.instance.LoadCurrentLevel();

        canMove = true;
    }

    //Reset the scene (e.g. when you lose)
    public IEnumerator RestartGame()
    {
        sound.FadeOut(LevelManager.instance.GetMusicName(), 1.5f); //Fade out music
        StartCoroutine(BattleUI.instance.FadeTo(Color.black, 1.5f)); //Fade screen to black
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reload this scene
    }

    //After totem moved, put move on cooldown
    public void TotemMoved()
    {
       StartCoroutine(MoveCooldown(BattleUI.instance.buildPhase ? 0.5f : MOVE_COOLDOWN));
    }

    private IEnumerator MoveCooldown(float cooldown)
    {
        //Set canMove=false until cooldown is over
        canMove = false;
        moveTimer = 0f;
        while(moveTimer < cooldown)
        {
            moveTimer = Mathf.Min(cooldown, moveTimer + Time.deltaTime);
            BattleUI.instance.cursor.SetMoveCooldown((cooldown - moveTimer)/ cooldown); //Show cooldown on cursor
            yield return null;
        }
        BattleUI.instance.cursor.SetMoveCooldown(0);
        canMove = true;
    }

    //Load next level
    public void NextLevel()
    {
        sound.FadeOut(LevelManager.instance.GetMusicName(), 1.5f); //Fade out music
        LevelManager.instance.IncrementLevel(); //Increase level count and reload scene
        StartCoroutine(RestartGame());
    }
}
