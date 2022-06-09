using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//HUD element which shows info of totem currently hovered over
public class InfoPanel : MonoBehaviour
{
    [HideInInspector]
    public Totem activeTotem;

    [SerializeField]
    private TextMeshProUGUI nameLabel;

    [SerializeField]
    private TextMeshProUGUI ritualDesc;

    [SerializeField]
    private Slider healthbar;

    [SerializeField]
    private Slider ritualProgress;

    [SerializeField]
    private TextMeshProUGUI canMove;

    private void Start()
    {
        Disable();
    }

    //Set values of bars on the panel
    public void UpdateInfo(float health, float ritual)
    {
        healthbar.value = health;
        ritualProgress.value = ritual;

        canMove.gameObject.SetActive(ritual >= 1); //Show a "can move" alert when totem can move to other pole

    }

    //Set the totem which is displayed on the panel
    public void SetActiveTotem(Totem t, string conditionDesc)
    {
        activeTotem = t;
        gameObject.SetActive(true);
        nameLabel.text = t.name;
        ritualDesc.text = conditionDesc;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
