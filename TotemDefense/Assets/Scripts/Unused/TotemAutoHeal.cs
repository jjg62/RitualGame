using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Experimental, old script for automatically healing this totem at an interval
public class TotemAutoHeal : MonoBehaviour
{
    private Totem totem;
    private const float HEAL_INTERVAL = 5f;
    private float timer;

    private void Start()
    {
        totem = GetComponent<Totem>();
        timer = HEAL_INTERVAL;
    }

    public void TickTimer()
    {
        timer -= Time.deltaTime;
    }

    public void Update()
    {
        if(timer < 0)
        {
            Debug.Log("HEAL");
            timer = HEAL_INTERVAL;
            Heal();
        }
    }

    private void Heal()
    {
        totem.SetHealth(totem.GetHealth() + totem.maxHealth/8);
    }
}
