using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{

    [SerializeField] PlayerRole[] Players;
    [SerializeField] GameObject ObjectBall;
    private Ball ball;
    private Vector3 target = Vector3.zero;
    public bool Gamestart = false;
   // public float BotSpeed = 2f;
    private void Start()
    {
        ball = ObjectBall.GetComponent<Ball>();

    }



    private void Update()
    {
        //CheckOtherPosWithRank();
            CheckOtherPos();
    }
    void CheckOtherPosWithRank()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (Players[i] != ball.GetPlayer())
            {
                if (Players[i].PlayerPos == PlayerRole.Position.RB )
                {
                    if (RBPosRank(Players[i]))
                    {
                        target= new Vector3(Random.Range(ball.GetPlayer().transform.position.x + 85, ball.GetPlayer().transform.position.x + 150), this.transform.position.y, Random.Range(ball.GetPlayer().transform.position.z + 200, ball.GetPlayer().transform.position.z + 15));
                        Players[i].transform.position = Vector3.Lerp(Players[i].transform.position, new Vector3(Mathf.Clamp(target.x, -250, 250), target.y, Mathf.Clamp(target.z, -200, 205)), Time.deltaTime );
                    }
                }
                else if (Players[i].PlayerPos == PlayerRole.Position.LB)
                {
                    if (LBPosRank(Players[i]))
                    {

                        target = new Vector3(Random.Range(ball.GetPlayer().transform.position.x + 85, ball.GetPlayer().transform.position.x + 150), this.transform.position.y, Random.Range(ball.GetPlayer().transform.position.z - 200, ball.GetPlayer().transform.position.z - 15));
                        Players[i].transform.position = Vector3.Lerp(Players[i].transform.position, new Vector3(Mathf.Clamp(target.x, -250, 250), target.y, Mathf.Clamp(target.z, -200, 205)), Time.deltaTime );
                    }
                }
                else if (Players[i].PlayerPos == PlayerRole.Position.LF)
                {
                    if (LFPosRank(Players[i]))
                    {

                        target = new Vector3(Random.Range(ball.GetPlayer().transform.position.x - 85, ball.GetPlayer().transform.position.x - 150), this.transform.position.y, Random.Range(ball.GetPlayer().transform.position.z - 200, ball.GetPlayer().transform.position.z - 15));
                        Players[i].transform.position = Vector3.Lerp(Players[i].transform.position,new Vector3 ( Mathf.Clamp(target.x, -250, 250),target.y,Mathf.Clamp(target.z, -200, 205)), Time.deltaTime );
                    }
                }
                else if (Players[i].PlayerPos == PlayerRole.Position.RF)
                {
                    if (RFPosRank(Players[i]))
                    {
                        target = new Vector3(Random.Range(ball.GetPlayer().transform.position.x - 85, ball.GetPlayer().transform.position.x - 150), this.transform.position.y, Random.Range(ball.GetPlayer().transform.position.z + 200, ball.GetPlayer().transform.position.z + 15));
                        Players[i].transform.position = Vector3.Lerp(Players[i].transform.position, new Vector3(Mathf.Clamp(target.x, -250, 250), target.y, Mathf.Clamp(target.z, -200, 205)), Time.deltaTime );
                    }
                }

                else if (Players[i].PlayerPos == PlayerRole.Position.AMC)
                {
                    PlayerRole temp = ball.GetPlayer().GetComponent<PlayerRole>();
                    Players[i].PlayerPos = temp.PlayerPos;
                    ball.GetPlayer().GetComponent<PlayerRole>().PlayerPos = PlayerRole.Position.AMC;
                }


            }

        }
    }
    void CheckOtherPos()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (Players[i] != ball.GetPlayer())
            {
                if (Players[i].PlayerPos == PlayerRole.Position.RB)
                {
                    if (DefPos(Players[i]))
                    {
                        target = new Vector3(ball.GetPlayer().transform.position.x + 120, this.transform.position.y, Players[i].transform.position.z);
                        if (!DesignatedRightWingZone(Players[i]))
                        {
                            target = new Vector3(target.x, target.y, Random.Range(90, 170));
                        }

                        Players[i].transform.position = Vector3.Lerp(Players[i].transform.position, new Vector3(Mathf.Clamp(target.x, -250, 250), target.y,target.z), Time.smoothDeltaTime );

                        if(GameManager.instance.PLayerToPass() == Players[i])
                        {
                            GameManager.instance.SetPassPoint(target);
                        }


                    }
                    

                }
                else if (Players[i].PlayerPos == PlayerRole.Position.LB)
                {
                    if (DefPos(Players[i]))
                    {

                        target = new Vector3(ball.GetPlayer().transform.position.x + 120, this.transform.position.y, Players[i].transform.position.z);
                        if (!DesignatedLeftWingZone(Players[i]))
                        {
                            target = new Vector3(target.x, target.y, Random.Range(-90, -170));
                        }

                        Players[i].transform.position = Vector3.Lerp(Players[i].transform.position, new Vector3(Mathf.Clamp(target.x, -250, 250), target.y, target.z), Time.smoothDeltaTime );
                        if (GameManager.instance.PLayerToPass() == Players[i])
                        {
                            GameManager.instance.SetPassPoint(target);
                        }



                    }
                }
                else if (Players[i].PlayerPos == PlayerRole.Position.LF &&(ball.GetPlayer().transform.position.x < -10 || ball.GetPlayer().transform.position.x > 10 || Gamestart))
                {
                    Gamestart = true;
                    if (ForPos(Players[i]))
                    {

                        target = new Vector3(ball.GetPlayer().transform.position.x - 150, this.transform.position.y, Players[i].transform.position.z);
                        if (!DesignatedLeftWingZone(Players[i]))
                        {
                            target = new Vector3(target.x, target.y, Random.Range(-170, -90));
                        }

                        Players[i].transform.position = Vector3.Lerp(Players[i].transform.position, new Vector3(Mathf.Clamp(target.x, -250, 250), target.y, target.z), Time.smoothDeltaTime );
                        if (GameManager.instance.PLayerToPass() == Players[i])
                        {
                            GameManager.instance.SetPassPoint(target);
                        }



                    }
                }
                else if (Players[i].PlayerPos == PlayerRole.Position.RF && (ball.GetPlayer().transform.position.x < -10 || ball.GetPlayer().transform.position.x > 10 ||  Gamestart))
                {
                    Gamestart = true;
                    if (ForPos(Players[i]))
                    {
                        target = new Vector3(ball.GetPlayer().transform.position.x - 150, this.transform.position.y, Players[i].transform.position.z);
                       
                        if (!DesignatedRightWingZone(Players[i]))
                        {
                            target =  new Vector3(target.x, target.y, Random.Range(90, 170));
                        }

                        Players[i].transform.position = Vector3.Lerp(Players[i].transform.position, new Vector3(Mathf.Clamp(target.x, -250, 250), target.y, target.z), Time.smoothDeltaTime );

                        if (GameManager.instance.PLayerToPass() == Players[i])
                        {
                            GameManager.instance.SetPassPoint(target);
                        }



                    }
                }

                else if (Players[i].PlayerPos == PlayerRole.Position.AMC)
                {
                    if (MidPos(Players[i]))
                    {
                        target = new Vector3(Random.Range(ball.GetPlayer().transform.position.x - 85, ball.GetPlayer().transform.position.x + 85), this.transform.position.y, Random.Range(-70, 70));
                        if (!DesignatedRightWingZone(Players[i]))
                        {
                            target = new Vector3(target.x, target.y, Random.Range(-70, 70));
                        }
                        Players[i].transform.position = Vector3.Lerp(Players[i].transform.position, new Vector3(Mathf.Clamp(target.x, -250, 250), target.y, target.z), Time.deltaTime/3);
                        
                        if (GameManager.instance.PLayerToPass() == Players[i])
                        {
                            GameManager.instance.SetPassPoint(target);
                        }

                    }


                }


            }

        }
    }
    private bool ForPos(PlayerRole player)
    {
        

       
        if (player.GetComponent<PlayerController>().enabled==false &&player.transform.position.x - ball.GetPlayer().GetComponent<PlayerController>().transform.position.x > -80)
        {
            return true;
        }

        else if (player.GetComponent<PlayerController>().enabled == false && player.transform.position.x-ball.GetPlayer().GetComponent<PlayerController>().transform.position.x < -120)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    private bool DefPos(PlayerRole player)
    {
        if(player.GetComponent<PlayerController>().enabled == false && player.transform.position.x  - ball.GetPlayer().GetComponent<PlayerController>().transform.position.x < 80 )
        {
            return true;
        }
     
        else if (player.GetComponent<PlayerController>().enabled == false && player.transform.position.x-ball.GetPlayer().GetComponent<PlayerController>().transform.position.x  > 120   )
        {

            return true;
        }
        else
        {
            return false;
        }
    }
    private bool MidPos(PlayerRole player)
    {
        if ( Mathf.Abs(ball.GetPlayer().transform.position.x-player.transform.position.x)<90)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    private bool DesignatedMidZone(PlayerRole player)
    {
        //50 -50
        if (player.transform.position.z > -50)
        {
            return false;
        }
        else if (player.transform.position.z < 50)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    private bool DesignatedLeftWingZone(PlayerRole player)
    {
        //-90 -170

        if (player.transform.position.z > -90)
        {
            return false;
        }
        else if(player.transform.position.z< -170)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool DesignatedRightWingZone(PlayerRole player)
    {
        //90 170
        if (player.transform.position.z < 90)
        {
            return false;
        }
        else if (player.transform.position.z > 170)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool RBPosRank(PlayerRole player)
    {
        if ((Mathf.Abs(Vector3.Distance(ball.GetPlayer().transform.position, player.transform.position)) < 130f))
        {
            return true;
        }
       else if ((player.transform.position.x < ball.GetPlayer().transform.position.x + 150 && player.transform.position.x + 85 > ball.GetPlayer().transform.position.x) && (player.transform.position.z - 100 < ball.GetPlayer().transform.position.z && player.transform.position.z > ball.GetPlayer().transform.position.z - 45))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool LBPosRank(PlayerRole player)
    {
        if ((Mathf.Abs(Vector3.Distance(ball.GetPlayer().transform.position, player.transform.position)) < 130f))
        {
            return true;
        }
        else if ((player.transform.position.x < ball.GetPlayer().transform.position.x + 150 && player.transform.position.x + 85 > ball.GetPlayer().transform.position.x )&& (player.transform.position.z  < ball.GetPlayer().transform.position.z + 100 && player.transform.position.z + 45 > ball.GetPlayer().transform.position.z ))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool LFPosRank(PlayerRole player)
    {
        if ((Mathf.Abs(Vector3.Distance(ball.GetPlayer().transform.position, player.transform.position)) < 130f))
        {
            return true;
        }
        else if ((player.transform.position.x - 150 < ball.GetPlayer().transform.position.x  && player.transform.position.x  > ball.GetPlayer().transform.position.x -85) && (player.transform.position.z < ball.GetPlayer().transform.position.z + 100 && player.transform.position.z + 45 > ball.GetPlayer().transform.position.z ))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool RFPosRank(PlayerRole player)
    {

        Debug.Log(Mathf.Abs(Vector3.Distance(ball.GetPlayer().transform.position, player.transform.position)));

        if((Mathf.Abs(Vector3.Distance(ball.GetPlayer().transform.position, player.transform.position)) < 130f)){
            return true;
        }
        else if (((player.transform.position.x - 150 < ball.GetPlayer().transform.position.x && player.transform.position.x > ball.GetPlayer().transform.position.x - 85) && !((player.transform.position.z < ball.GetPlayer().transform.position.z+100  && player.transform.position.z + 45> ball.GetPlayer().transform.position.z ))))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
public partial class SROptions
{
    private float _AIMoveSpeed = 100;

    [Category("AI Move Speed")]
    public float AIMoveSpeed
    {
        get { return _AIMoveSpeed; }
        set { _AIMoveSpeed = value; }
    }
   
}