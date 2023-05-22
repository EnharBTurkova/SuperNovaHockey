using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMCStateMachine : BaseStateMachine
{
    public enum StrikerState
    {
        OnBall,
        Defending,
        Attacking,
    }
    private Vector3 target = Vector3.zero;
    private StrikerState currentState;

    private void Update()
    {
        switch (currentState)
        {

            case StrikerState.OnBall:
                GetComponent<PlayerController>().enabled = true;
                GetComponent<PlayerController>().SelectionRingShow();
                break;

            case StrikerState.Defending:
                GetComponent<PlayerController>().enabled = false;
                
                break;

            case StrikerState.Attacking:
                CheckAttackPos();
                break;
            default:
                // Add a default case to handle unexpected states
                break;
        }
    }


    public override void CheckAttackPos()
    {
        GetComponent<PlayerController>().enabled = false;
        if (GetBackYourPos())
        {
            target = new Vector3(Random.Range(ball.GetPlayer().transform.position.x - 85, ball.GetPlayer().transform.position.x + 85), this.transform.position.y, Random.Range(-70, 70));
            if (!GetBackYourSide())
            {
                target = new Vector3(target.x, target.y, Random.Range(-70, 70));
            }
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(Mathf.Clamp(target.x, -250, 250), target.y, target.z), Time.deltaTime / 3);


        }
    }
    public override void SetOnBallState()
    {
        if (currentState != StrikerState.OnBall)
        {
            currentState = StrikerState.OnBall;
            // Add code here to handle the behavior when transitioning to the Defending state for the striker
        }
    }
    public override void SetDefendingState()
    {
        if (currentState != StrikerState.Defending)
        {
            currentState = StrikerState.Defending;
            // Add code here to handle the behavior when transitioning to the Defending state for the striker
        }
    }
    public override void SetAttackingState()
    {
        if (currentState != StrikerState.Attacking)
        {
            currentState = StrikerState.Attacking;
            // Add code here to handle the behavior when transitioning to the Attacking state for the striker
        }
    }
    public override bool GetBackYourSide()
    {
        //50 -50
        if (this.transform.position.z > -50)
        {
            return false;
        }
        else if (this.transform.position.z < 50)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public override bool GetBackYourPos()
    {

        if (Mathf.Abs(ball.GetPlayer().transform.position.x - this.transform.position.x) < 90)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
