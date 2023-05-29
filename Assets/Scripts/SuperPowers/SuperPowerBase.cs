using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SuperPowerBase : MonoBehaviour
{
    public abstract void IncreaseMana();
    public abstract void ResetMana();
    public abstract bool GetCanUse();
    public abstract void SetCanUse(bool canuse);
    
}
