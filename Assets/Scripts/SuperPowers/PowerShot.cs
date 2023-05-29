using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerShot : SuperPowerBase
{


    [SerializeField] Image PowerShotImage;
    public float Mana;
    private bool canUse;

    private void Update()
    {


        if(Mana/100 >= 1)
        {
            SetCanUse(true);
        }
        else
        {
            SetCanUse(false);
        }
    }
    public override void SetCanUse(bool flag)
    {
        canUse = flag;
    }
    public override bool GetCanUse()
    {
        return canUse;
    }
    public override void IncreaseMana()
    {
        Mana += 10;
        PowerShotImage.fillAmount = Mana / 100;
    }
    public override void ResetMana()
    {
        Mana = 0;
        PowerShotImage.fillAmount = Mana / 100;
    }
}
