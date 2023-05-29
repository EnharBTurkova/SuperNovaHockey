using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    [SerializeField] BaseStateMachine[] Players;
    [SerializeField] Ball ball;

    private void Update()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (ball.GetPlayer()!= null && ball.GetPlayer().GetComponent<PlayerController>()!= null)
            {

                if (Players[i].gameObject != ball.GetPlayer())
                {
                    Players[i].SetAttackingState();
                }   
                else
                {
                    Players[i].SetOnBallState();
                }
            }

        }
    }
}
