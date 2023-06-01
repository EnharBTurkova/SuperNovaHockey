using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : MonoBehaviour
{
    public GameObject TacklePlayer;
    public float DribbleSpped;
    public float PassSpeed;
    public float ShootPower;
    public GameObject goal;
    public GameObject PlayerToPass;
    public Ball ball;
    public bool Gamestart = false;

    public abstract void Pass(GameObject PlayerToPass);
  
    public abstract void Defence();
    public abstract void Shoot();
    public abstract void Dribble();
    public abstract void MoveToBall();
    public abstract void CheckAttackPos();
    public abstract void SetDefendingState();
    public abstract void SetAttackingState();
    public abstract void SetDribbleState();
    public abstract void SetPassState();
    public abstract void SetShootState();
    public abstract void SetMoveToBallState();
    public abstract bool IsPlayerInPassDirection(Vector3 passDirection, float maxDistance);
    public abstract void SetTackleState();
    public abstract bool GetBackYourSide();
    public abstract bool GetBackYourPos();
    public abstract void SetTacklePlayer(GameObject tacklePlayer);
    public abstract GameObject GetTacklePlayer();
    public abstract void MoveToBallTrue();
    public abstract void MoveToBallFalse();
    public abstract void PressPlayerTrue();
    public abstract void PressPlayerFalse();
    public abstract GameObject GetTargetPlayer();
    public abstract void SetDefenceDistance(int distance);

    public abstract void Tackle(GameObject tacklePlayer);
}
