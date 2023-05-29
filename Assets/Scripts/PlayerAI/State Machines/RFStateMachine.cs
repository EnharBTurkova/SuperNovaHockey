using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFStateMachine : BaseStateMachine
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
        if (ball.GetPlayer() != null)
        {
            if ((ball.GetPlayer().transform.position.x < -10 || ball.GetPlayer().transform.position.x > 10 || Gamestart))
            {
                Gamestart = true;
                if (GetBackYourPos())
                {
                    target = new Vector3(ball.GetPlayer().transform.position.x - 150, this.transform.position.y, this.transform.position.z);

                    if (!GetBackYourSide())
                    {
                        target = new Vector3(target.x, target.y, Random.Range(90, 170));
                    }

                    this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(Mathf.Clamp(target.x, -250, 250), target.y, target.z), Time.smoothDeltaTime);
                }
            }
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
        if (this.transform.position.z < 90)
        {
            return false;
        }
        else if (this.transform.position.z > 170)
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

        if (this.GetComponent<PlayerController>().enabled == false && this.transform.position.x - ball.GetPlayer().GetComponent<PlayerController>().transform.position.x > -80)
        {
            return true;
        }

        else if (this.GetComponent<PlayerController>().enabled == false && this.transform.position.x - ball.GetPlayer().GetComponent<PlayerController>().transform.position.x < -120)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
