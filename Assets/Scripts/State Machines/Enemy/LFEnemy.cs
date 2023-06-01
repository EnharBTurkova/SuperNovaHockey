using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LFEnemy : EnemyBaseState
{
    public enum StrikerState
    {
        MoveToBall,
        Tackle,
        Dribble,
        Pass,
        Shoot,
        Defending,
        Attacking,
    }
    [SerializeField] GameObject[] opposingPlayers;
    [SerializeField] GameObject targetPlayer;
    private AIPath aipath;
    private bool BallOnYourAttacker;
    private Vector3 target = Vector3.zero;
    private StrikerState currentState;
    private bool goToBall = false;
    private int defenceDistance = 20;
    private void Start()
    {
        aipath = GetComponent<AIPath>();
    }
    private void Update()
    {
        switch (currentState)
        {
            case StrikerState.MoveToBall:
                MoveToBall();
                break;
            case StrikerState.Dribble:
                Dribble();
                break;
            case StrikerState.Pass:
                Pass(PlayerToPass);
                break;
            case StrikerState.Shoot:
                Shoot();
                break;
            case StrikerState.Defending:
                Defence();
                break;
            case StrikerState.Attacking:
                CheckAttackPos();
                break;
            case StrikerState.Tackle:
                Tackle(TacklePlayer);
                break;
            default:
                // Add a default case to handle unexpected states
                break;
        }
     
    }
    public override void MoveToBall()
    {
        Vector3 ballDirection = ball.transform.position - transform.position;
        ballDirection.y = 0f; // Ignore vertical component

        // Rotate towards the goal target
        Quaternion targetRotation = Quaternion.LookRotation(ballDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        // Move towards the goal target
        Vector3 movement = transform.forward * DribbleSpped * Time.deltaTime;
        transform.position += movement;
    }
    public override void Tackle(GameObject TacklePlayer)
    {
        Vector3 pushDirection = TacklePlayer.transform.position - transform.position;

        Quaternion newRotation = Quaternion.LookRotation(pushDirection, Vector3.up);



        CameraFollow.instance.CameraShaketrue();

        pushDirection.Normalize();
        TacklePlayer.GetComponent<Rigidbody>().AddForce(pushDirection * 100, ForceMode.Impulse);
        ball.StickPlayer = false;

        ball.transform.position = this.GetComponent<Enemy>().BallLocation.position;
        ball.SetPlayer(this.transform);
        ball.SetPlayerBallPosition(this.GetComponent<Enemy>().BallLocation);

    }
    public override void Defence()
    {

        Vector3 DefenceDirection = targetPlayer.transform.position  - transform.position;

        DefenceDirection -= Vector3.one * defenceDistance;
        DefenceDirection.y = 0f; // Ignore vertical component
        // Rotate towards the goal target
        Quaternion targetRotation = Quaternion.LookRotation(DefenceDirection );
        transform.rotation = targetRotation;

        // Move towards the goal target
        Vector3 movement = transform.forward * DribbleSpped * Time.deltaTime;
        transform.position += movement;
    }
 
    public override void Pass(GameObject PlayerToPass)
    {
       // Debug.Log("Pass");
        GameManager.instance.shoottakentrue();
        var force = Vector3.zero;

        if (PlayerToPass.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
          
            force = ball.GetBallLocation().position - (PlayerToPass.transform.position );
        }
        else
        {
            Debug.DrawLine(ball.GetBallLocation().position, PlayerToPass.transform.position, Color.blue, 1000f);
            force = ball.GetBallLocation().position - PlayerToPass.transform.position;

        }
      
        ball.StickPlayer = false;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().AddForce(-force.normalized * PassSpeed * 10000 * Time.fixedDeltaTime);
    }
    public override void Dribble()
    {
//        Debug.Log("dribbleee");
        Vector3 goalDirection = goal.transform.position - transform.position;
        goalDirection.y = 0f; // Ignore vertical component


        aipath.destination = goalDirection;

    }
    public override void Shoot()
    {
        Debug.Log("shoot");
        GameManager.instance.shoottakentrue();

        Vector3 goalDirection = goal.transform.position - transform.position;
        goalDirection.y = 0f; // Ignore vertical component

        // Rotate towards the goal target
        Quaternion targetRotation = Quaternion.LookRotation(goalDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        ball.GetComponent<Ball>().StickPlayer = false;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().AddForce(goalDirection.normalized * ShootPower * 10000 * Time.fixedDeltaTime);
    }

    public override void CheckAttackPos()
    {
      
        if (GetBackYourPos())
        {
            target = new Vector3(ball.GetPlayer().transform.position.x + 120, this.transform.position.y, this.transform.position.z);
            if (!GetBackYourSide())
            {
                target = new Vector3(target.x, target.y, Random.Range(90, 170));
            }

            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(Mathf.Clamp(target.x, -250, 250), target.y, target.z), Time.smoothDeltaTime);
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

        if ( this.transform.position.x - ball.GetPlayer().transform.position.x > 80)
        {
            return true;
        }

        else if (this.transform.position.x - ball.GetPlayer().transform.position.x < 120)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override void SetMoveToBallState()
    {
        if (currentState != StrikerState.MoveToBall)
        {
            currentState = StrikerState.MoveToBall;
            // Add code here to handle the behavior when transitioning to the Attacking state for the striker
        }
    }
    public override void SetTackleState()
    {
        if (currentState != StrikerState.Tackle)
        {
            currentState = StrikerState.Tackle;
            // Add code here to handle the behavior when transitioning to the Defending state for the striker
        }
    }
    public override void SetDribbleState()
    {
        if (currentState != StrikerState.Dribble)
        {
            currentState = StrikerState.Dribble;
            // Add code here to handle the behavior when transitioning to the Defending state for the striker
        }
    }
    public override void SetPassState()
    {
        if (currentState != StrikerState.Pass)
        {
            currentState = StrikerState.Pass;
            // Add code here to handle the behavior when transitioning to the Defending state for the striker
        }
    }
    public override void SetShootState()
    {
        if (currentState != StrikerState.Shoot)
        {
            currentState = StrikerState.Shoot;
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
    public override void MoveToBallFalse()
    {
        goToBall = false;
    }
    public override void MoveToBallTrue()
    {
        goToBall = true;
    }
    public override void PressPlayerFalse()
    {
        BallOnYourAttacker = false;
    }
    public override void PressPlayerTrue()
    {
        BallOnYourAttacker = true;
    }

    public override void SetTacklePlayer(GameObject tacklePlayer)
    {
        TacklePlayer = tacklePlayer;
    }
    public override GameObject GetTacklePlayer()
    {
        return TacklePlayer;
    }
    public override GameObject GetTargetPlayer()
    {
        return targetPlayer;
    }
    public override void SetDefenceDistance(int distance)
    {
        if (defenceDistance >= 10)
        {
            defenceDistance -= distance;
        }
    }
    public override bool IsPlayerInPassDirection(Vector3 passDirection, float maxDistance)
    {
        // Get all opposing players in the scene


        // Iterate through each opposing player
        foreach (GameObject player in opposingPlayers)
        {
            // Calculate the vector from the AI-controlled player to the opposing player
            Vector3 playerDirection = player.transform.position - transform.position;

            // Check if the opposing player is in the pass direction
            float dotProduct = Vector3.Dot(passDirection.normalized, playerDirection.normalized);
            if (dotProduct > 0 && playerDirection.magnitude <= maxDistance)
            {
                // There is a player in the pass direction
                return true;
            }
        }

        // No player found in the pass direction
        return false;
    }


}
