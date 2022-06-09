using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Allows levels to be designed in inspector, and sets up each level
public class LevelManager : MonoBehaviour
{
    //Singleton
    #region Singleton
    public static LevelManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
    }
    #endregion

    [SerializeField]
    private int currentID;

    [SerializeField]
    private Level[] levels;

    private WaveSpawner spawner;
    private TotemStack leftStack;
    private TotemStack rightStack;
    private Transform buyMenu;

    public void LoadCurrentLevel()
    {
        LoadLevel(currentID);
        Game.instance.sound.Play(GetMusicName());
    }

    public void IncrementLevel()
    {
        currentID++;
    }

    public void LoadLevel(int id)
    {
        if (id < levels.Length)
        {
            //Find spawner and buymenu in room
            spawner = GameObject.FindWithTag("Spawner").GetComponent<WaveSpawner>();
            buyMenu = GameObject.FindWithTag("Buy Menu").transform;
            leftStack = GameObject.FindWithTag("Left Stack").GetComponent<TotemStack>();
            rightStack = GameObject.FindWithTag("Right Stack").GetComponent<TotemStack>();
 

            //Set up Buy Menu
            List<BuyTotem> buyInstances = new List<BuyTotem>();
            for (int i = levels[id].buys.Length - 1; i >= 0; i--)
            {
                //Instantiate each new totem buy
                BuyTotem t = levels[id].buys[i];
                BuyTotem newBuy = Instantiate(t, buyMenu);
                newBuy.transform.localPosition = new Vector3(-450 + i * 150, 6.7f, 0);

                buyInstances.Add(newBuy);
            }

            //Place Starting Totems
            foreach(Totem t in levels[id].leftStartingTotems)
            {
                leftStack.AddTotem(t);
            }

            foreach (Totem t in levels[id].rightStartingTotems)
            {
                rightStack.AddTotem(t);
            }

            Game.instance.centre.lives = 1;
            currentID = id;;

            StartCoroutine(StartSpawning(spawner, id, buyInstances.ToArray(), levels[id].buildPhases));
        }
    }

    IEnumerator StartSpawning(WaveSpawner sp, int id, BuyTotem[] buys, BuildPhase[] buildPhases)
    {
        //After a delay, activate the spawner
        yield return new WaitForSeconds(2f);
        //Extra tutorial message before 3rd level
        if (id == 3) BattleUI.instance.ShowTutorialMessage("Looks like we got them all, for now. Use this time to prepare for the next wave - any Totems you place now will cost half the price they would if you placed them mid-battle!\n\nUse the Hints given by the Shrine to decide which Totems to place on each side, build them, then press Ready in the bottom-right.");
        spawner.Begin(levels[id].waves, levels[id].messages, buys, buildPhases, levels[id].startingSouls);
    }

    public string GetMusicName()
    {
        return levels[currentID].music;
    }

    //One-Off Tutorials
    public bool BlastTutorial;

}
