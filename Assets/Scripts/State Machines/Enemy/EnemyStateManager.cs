using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] EnemyBaseState[] Players;
    [SerializeField] GameObject[] Enemy;
    [SerializeField] GameObject goal;
    [SerializeField] Ball ball;
    [SerializeField] float TackleDistance;
    private float tackleTimer = 3f;
    private bool canTackle;

    private void Update()
    {
        if (tackleTimer <= 0)
        {
            canTackle = true;
        }
        else
        {
            tackleTimer -= Time.deltaTime;
        }
        for (int i = 0; i < Players.Length; i++)
        {
           
            if (ball.GetPlayer() != null)
            {
              if (ball.GetPlayer().GetComponent<Enemy>() != null)
                {
                    if (Players[i].gameObject != ball.GetPlayer())
                    {
                        Players[i].SetAttackingState();
                    }
                    else
                    {
                        Players[i].GetComponent<EnemyBaseState>().MoveToBallFalse();
                        // topun yeni bir enemye ilk geldiği an:
                        // 1- Senin x inden büyük x i olan takım arkadaşın var mı varsa ona pas()
                        // 2- x + 30 un veya daha yakınında rakip var mı yoksa 50f topu sür sonra tekrar kontrol et
                        // 3- x + 30 veya daha yakınında rakip varsa Sahanın neresinde olduğunu kontrol et 
                        // 4- eğer x in 0 dan büyükse shoot() şansını dene
                        // 5- eğer x in 0 dan küçükse rastgele bir arkadaşına pas ver baskıyı azalt





                        for (int j = 0; j < Players.Length; j++)
                            {
                                if (Players[j].gameObject != ball.GetPlayer())
                                {
                                  
                                    if (Players[j].transform.position.x > ball.GetPlayer().transform.position.x + 20f)
                                    { //önünde takım arkadaşın var pas ver
                                       // Debug.Log("Önünde Takım arkadaşın var");
                                 
                                       bool isavailable = ball.GetPlayer().GetComponent<EnemyBaseState>().IsPlayerInPassDirection(Players[j].gameObject.transform.position-ball.GetPlayer().transform.position,100);
                                    if (!isavailable)
                                    {
                                            ball.GetPlayer().GetComponent<EnemyBaseState>().PlayerToPass = Players[j].gameObject;
                                         
                                            ball.GetPlayer().GetComponent<EnemyBaseState>().SetPassState();

                                    }
                                    else
                                    {

                                        ball.GetPlayer().GetComponent<EnemyBaseState>().PlayerToPass = FindAnotherPlayer(Players[j].gameObject);
                                    
                                        ball.GetPlayer().GetComponent<EnemyBaseState>().SetPassState();
                                    }
                                        break;
                                    }
                                    else
                                    {
                                      
                                        for (int k = 0; k < Enemy.Length; k++)
                                        {
                                            if (Enemy[k].transform.position.x<ball.GetPlayer().transform.position.x +30 && Enemy[k].transform.position.x > ball.GetPlayer().transform.position.x)
                                            { // 30f yakınında adam var
                                                //Debug.Log("30f yakınında rakip var");
                                                if (ball.GetPlayer().transform.position.x > 0)
                                                {
                                                    // rakip yarı sahadasın ve baskı altındasın önünde de takım arkadaşın yok şut çek
                                                    //Debug.Log("30f yakınında rakip var ve rakip sahadasın şut çek");
                                                    ball.GetPlayer().GetComponent<EnemyBaseState>().SetShootState();
                                                    break;
                                                }
                                                else
                                                {// kendi yarı sahandasın ve baskı altındasın önünde de takım arkadaşın yok herhangi birine pas ver
                                                   // Debug.Log("30f yakınında rakip var ve kendi sahadasın pas ver");
                                                    ball.GetPlayer().GetComponent<EnemyBaseState>().PlayerToPass = Players[Random.Range(0, Players.Length)].gameObject;
                                                    ball.GetPlayer().GetComponent<EnemyBaseState>().SetPassState();
                                                    break;
                                                }
                                            }
                                            else
                                            { // 30f yakınında adam yok önün boş 50f top sür
                                               

                                                if(Mathf.Abs(ball.GetPlayer().transform.position.x - goal.transform.position.x) < 200f)
                                                {
                                                    //kaleye 50ften yakınsın şut çek
                                                   // Debug.Log("Önün boş kaleye yakınsın çek şutunu");
                                                    ball.GetPlayer().GetComponent<EnemyBaseState>().SetShootState();
                                                    break;
                                                }
                                                else
                                                {
                                                    // kaleye uzaksın biraz top sür
                                                   // Debug.Log("top sür");
                                                    ball.GetPlayer().GetComponent<EnemyBaseState>().PlayerToPass = Players[Random.Range(0,Players.Length)].gameObject;
                                                    ball.GetPlayer().GetComponent<EnemyBaseState>().SetDribbleState();
                                                    break;

                                                }
                                            }

                                        }

                                    }
                                }
                            }
                        

                    }

                }
                else
                {
                    if(Players[i].GetTargetPlayer() == ball.GetPlayer())
                    {
                        Players[i].SetDefenceDistance(1);
                    }

                    if (Vector3.Distance(ball.GetPlayer().transform.position, Players[i].transform.position) < TackleDistance && canTackle)
                    {
                        Debug.Log("tackles");
                        canTackle = false;
                        tackleTimer = 2f;
                        Players[i].SetDefenceDistance(-10);
                        Players[i].SetTacklePlayer(ball.GetPlayer());
                        Players[i].SetTackleState();
                        TackleDistance = 10;
                    }
                    else
                    {
                        Players[i].SetDefendingState();
                    }
                 
                }
        }
            else
            {
                float closest = 100000f;
                GameObject closestplayer = null;
                for (int k= 0; k < Players.Length; k++)
                {
                    float temp = Vector3.Distance(Players[k].transform.position, ball.transform.position);
                    if (temp < closest)
                    {
                        temp = closest;
                        closestplayer = Players[k].gameObject;
                    }
                    
                }
                closestplayer.GetComponent<EnemyBaseState>().SetMoveToBallState();
            }

        }
    }



    public GameObject FindAnotherPlayer(GameObject notthis)
    {
        GameObject playertopass = null;
        for (int i = 0; i < Players.Length; i++)
        {
           
            if (Players[i] != notthis && Players[i] != ball.GetPlayer() )
            {
               
            
                if (Players[i].transform.position.x > ball.GetPlayer().transform.position.x)
                {
                    playertopass = Players[i].gameObject;
                    break;
                }
                else
                {
                    playertopass = Players[i].gameObject;
                }
            }
        }
        return playertopass;
    }
}
