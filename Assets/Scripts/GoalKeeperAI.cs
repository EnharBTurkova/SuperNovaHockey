using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalKeeperAI : MonoBehaviour
{
    [SerializeField] GameObject Ball;
    [SerializeField] GameObject Goal;
    [SerializeField] float MoveSpeed;
    private Vector3 targetPosition;
    private Animator anim;
    [SerializeField] Animator colliderAnim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
      
        StayBetweenBallandGoal();
      
        if (Vector3.Distance(Ball.transform.position, Goal.transform.position)<15f)
        {
            if (Ball.transform.position.z < 0 && this.transform.position.z > Ball.transform.position.z )
            {
            
            }
            else if(Ball.transform.position.z>0 && this.transform.position.z < Ball.transform.position.z)
            {
               
            }
        }
        if(Vector3.Distance(Ball.transform.position, this.transform.position) < 7f)
        {
            
        }
    }

    public void StayBetweenBallandGoal()
    {
        float currentDistance = Vector3.Distance(Ball.transform.position, Goal.transform.position);
        targetPosition = Ball.transform.position + (Goal.transform.position - Ball.transform.position).normalized;
  
        
        this.gameObject.transform.position = Vector3.MoveTowards(transform.position, new Vector3(this.transform.position.x, this.transform.position.y, Mathf.Clamp(targetPosition.z,-10f,10f)), MoveSpeed * Time.deltaTime);
    }

}
