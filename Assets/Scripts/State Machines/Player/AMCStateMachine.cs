using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AMCStateMachine : BaseStateMachine
{
    public enum StrikerState
    {
        OnBall,
        Defending,
        Attacking,
    }
    [SerializeField] GameObject targetPlayer;
    private AIPath aipath;
    private int defenceDistance = 5;
    private Vector3 target = Vector3.zero;
    private StrikerState currentState;
    private void Update()
    {
        if (this.transform.position.y < 13)
        {
            this.transform.position = new Vector3(this.transform.position.x, 14, this.transform.position.z);
        }
        switch (currentState)
        {

            case StrikerState.OnBall:
                GetComponent<PlayerController>().enabled = true;
                GetComponent<PlayerController>().SelectionRingShow();
                break;

            case StrikerState.Defending:
                GetComponent<PlayerController>().enabled = false;
                Defence();
                break;

            case StrikerState.Attacking:
                CheckAttackPos();
                break;
            default:
                // Add a default case to handle unexpected states
                break;
        }
    }

    public override void Defence()
    {

        Vector3 DefenceDirection = targetPlayer.transform.position - transform.position;
        DefenceDirection -= Vector3.one * defenceDistance;
        DefenceDirection.y = 0f; // Ignore vertical component
        // Rotate towards the goal target
        Quaternion targetRotation = Quaternion.LookRotation(DefenceDirection);
        transform.rotation = targetRotation;

        // Move towards the goal target
        Vector3 movement = transform.forward * 100 * Time.deltaTime;
        transform.position += movement;
    }
    public override void CheckAttackPos()
    {
        GetComponent<PlayerController>().enabled = false;
        if (ball.GetPlayer() != null)
        {
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
}
