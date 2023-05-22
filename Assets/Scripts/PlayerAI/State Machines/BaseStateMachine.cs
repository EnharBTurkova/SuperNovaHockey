using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour
{
    public Ball ball;
    public bool Gamestart = false;
    public abstract void CheckAttackPos();
    public abstract void SetDefendingState();
    public abstract void SetAttackingState();
    public abstract void SetOnBallState();
    public abstract bool GetBackYourSide();
    public abstract bool GetBackYourPos();
    
}
