using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawn a wave of enemies when the previous wave is partially defeated, dependent on a set threshold
public class WaveSpawner : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;

    [SerializeField]
    private int currentWave;

    private BuyTotem[] buys;
    private BuildPhase[] buildPhases; //Some waves will be build phases, giving a chance for player to rebuild totems


    [SerializeField]
    private Soul soulPref;

    private ShowMessages showMessages;

    private void Start()
    {
        showMessages = GetComponent<ShowMessages>();
    }

    //At the start of a level:
    public void Begin(Wave[] waves, MessageInfo[] messages, BuyTotem[] buys, BuildPhase[] buildPhases, int startingSouls)
    {
        //Get the waves and available buys for this level
        this.waves = waves;
        this.buys = buys;
        showMessages.messages = messages;
        this.buildPhases = buildPhases;

        Game.instance.AddSouls(startingSouls); //Give player starting souls

        //Start first wave
        currentWave = -1;
        NextWave();

        /*
        if (!buildPhase)
        {
            StartWave();
        }
        else
        {
            BattleUI.instance.ShowBuildPhase(true);
        }
        */
        
    }

    //After build phase, continue waves of enemies
    public void ContinueWaves()
    {
        int anytimeTotemAmount = 5; //How many of the totem buys can be used at any time
        BattleUI.instance.ShowBuildPhase(false);

        //Disable extra totem buys, double price of those that remain
        for (int i = 0; i < buys.Length; i++)
        {
            if (i > buys.Length - 1 - anytimeTotemAmount)
            {
                buys[i].DoublePrice(true);
            }
            else
            {
                buys[i].gameObject.SetActive(false);
            }

        }

        //Spawn the wave
        SpawnWave(waves[currentWave]);
        showMessages.Show(currentWave);
    }

    /*
    public void StartWave()
    {
        int anytimeTotemAmount = 1;
        BattleUI.instance.ShowBuildPhase(false);
        for(int i = 0; i < buys.Length; i++)
        {
            if (i > buys.Length - 1 - anytimeTotemAmount)
            {
                buys[i].DoublePrice(true);
            }
            else{
                Destroy(buys[i].gameObject);
            }

        }

        NextWave();
    }
    */

    //Go to next wave
    public void NextWave()
    {
        currentWave++;
        showMessages.Show(currentWave);
        if (currentWave < waves.Length)
        {
            //Check if this wave has a build phase
            foreach (BuildPhase bp in buildPhases)
            {
                if (bp.beforeWave == currentWave)
                {
                    SetupBuildPhase(bp);
                    return; //Stop until ContinueWaves() is called
                }
            }

            //Otherwise, spawn wave normally
            SpawnWave(waves[currentWave]);
        }
        else
        {
            //Level completed
            Game.instance.NextLevel();
        }
    }
        
    private void SetupBuildPhase(BuildPhase bp)
    {
        //Get Shrine Clouds
        ShrineCloud leftCloud = GameObject.FindWithTag("Left Cloud").GetComponent<ShrineCloud>();
        ShrineCloud rightCloud = GameObject.FindWithTag("Right Cloud").GetComponent<ShrineCloud>();

        //Set their warnings
        leftCloud.Setup(bp.leftWarning);
        rightCloud.Setup(bp.rightWarning);

        BattleUI.instance.ShowBuildPhase(true);

        //Allow all totem buys and return price to normal
        foreach (BuyTotem buy in buys)
        {
            buy.DoublePrice(false);
            buy.gameObject.SetActive(true);
        }
    }


    private void SpawnWave(Wave w)
    {
        w.spawner = this; //Give wave reference to this object
        w.StartWave();
        foreach (SpawnInfo sp in w.spawns)
        {
            StartCoroutine(SpawnEnemy(sp, w));
        }
    }

    //Spawn enemy after delay
    IEnumerator SpawnEnemy(SpawnInfo sp, Wave w)
    {
        yield return new WaitForSeconds(sp.spawnDelay);
        EnemySpawnAnimation spawned = Instantiate(sp.enemy);
        spawned.transform.position = (Vector2)transform.position + sp.spawnLocation;
        spawned.wave = w; //Give enemy spawner reference to wave
    }
}
