using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    [SerializeField] BaseStateMachine[] Players;
    [SerializeField] Ball ball;
    [SerializeField] float ChangingPlayerDistance = 3000f;
    [SerializeField] float closestDistance = 100f;
    [SerializeField] GameObject ClosestPlayer;
    private void Update()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (ball.GetPlayer()!= null)
            {
                
                if(ball.GetPlayer().GetComponent<Enemy>() == null)
                {
                
                    if (Players[i].gameObject != ball.GetPlayer() && ball.GetPlayer().GetComponent<PlayerController>() != null)
                    {
                        Players[i].SetAttackingState();
                    }   
                    else
                    {
                        Players[i].SetOnBallState();
                    }

                }
                else
                {
                    //Sürekli adam değiştirmemek için current closest player belli mesafe uzaklaşana kadar yeni bir closestplayer araması yapma.
                    if (ClosestPlayer == null || Vector3.Distance(ClosestPlayer.transform.position, ball.transform.position) > ChangingPlayerDistance)
                    {
                        for (int j = 0; j < Players.Length; j++)
                        {
                            if (Vector3.Distance(Players[j].transform.position, ball.transform.position) < closestDistance)
                            {
                                Players[j].SetOnBallState();
                                ClosestPlayer = Players[j].gameObject;
                                closestDistance = Vector3.Distance(Players[j].transform.position, ball.transform.position);

                            }

                        }
                    }
                    //ekstra önlem
                    if (ClosestPlayer != null && Players[i].gameObject != ClosestPlayer)
                    {

                        Players[i].SetDefendingState();
                    }
                }
            }
            else
              {
                    
                    for (int j = 0; j < Players.Length; j++)
                    {
                        //Sürekli adam değiştirmemek için current closest player belli mesafe uzaklaşana kadar yeni bir closestplayer araması yapmıyor.
                        if (ClosestPlayer == null || Vector3.Distance(ClosestPlayer.transform.position, ball.transform.position) > ChangingPlayerDistance )
                        {


                            if (Vector3.Distance(Players[j].transform.position, ball.transform.position) < closestDistance)
                            {
                                Debug.Log(Players[j] + "İs the closest");
                                Players[j].SetOnBallState();
                                ClosestPlayer = Players[j].gameObject;
                                closestDistance = Vector3.Distance(Players[j].transform.position, ball.transform.position);
                                
                            }
                        }
                    }
                    //ekstra önlem
                if (ClosestPlayer != null && Players[i].gameObject != ClosestPlayer)
                {
                    Debug.Log(Players[i] + "İnooooo");
                    Players[i].SetDefendingState();
                }
            }

        }
    }
}
