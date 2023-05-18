using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("ScoreBoard")]
    [SerializeField] Text[] TimeText;
    [SerializeField] Text HomeScoreText;
    [SerializeField] Text AwayScoreText;
    [SerializeField] Text RespawnText;
    [SerializeField] Text FinalScore;
    [SerializeField] Text ResultText;
    [SerializeField] PlayerAI ai;
  
    [Header("Game")]
    [SerializeField] GameObject Ball;
    [SerializeField] GameObject[] PlayerTeam;
    [SerializeField] GameObject Goal;
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] Transform BallSpawnPoint;

    [SerializeField] float RespawnTimer = 3.5f;
    private GameObject closestPlayer;
    private int HomeScore = 0;
    private int AwayScore = 0;
    private float GameTime = 90f;
    private bool isRestart;
    private bool isShotTaken;
    private Vector3 PassPoint;

    private void Start()
    {
        
        instance = this;
        Ball.SetActive(true);
    }
    void Update()
    {
        if (Ball.activeInHierarchy)
        {
            GameTime -= Time.deltaTime;
        }
        else
        {
            RespawnText.gameObject.SetActive(true);
            Time.timeScale = 0;
            if (RespawnTimer <= 0)
            {
                RespawnText.gameObject.SetActive(false);
                RestartGame();
                RespawnTimer = 3.5f;
            }

            RespawnTimer -= Time.unscaledDeltaTime;
            RespawnText.text = ((int)RespawnTimer).ToString();

        }
        for (int i = 0; i < TimeText.Length; i++)
        {
            TimeText[i].text = ((int)GameTime).ToString();

        }
        if (GameTime <= 0)
        {
            GameTime = 90f;
            //RestartGame();
           // Time.timeScale = 0f;
            //EndGame(HomeScore, AwayScore);
           // GameOverScreen.SetActive(true);
        }
      
            CheckTheClosestPlayer();


    }

    void CheckTheClosestPlayer()
    {
        
        float closest = 1000f;
        for (int i = 0; i < PlayerTeam.Length; i++)
        {
            float DistanceToBall = Vector3.Distance(PlayerTeam[i].transform.position, Ball.transform.position);
            if (DistanceToBall < closest)
            {
                closest = DistanceToBall;
                closestPlayer = PlayerTeam[i];
            }
        }
        for (int i = 0; i < PlayerTeam.Length; i++)
        {
            if (PlayerTeam[i] != closestPlayer && PlayerTeam[i] != Ball.GetComponent<Ball>().GetPlayer())
            {
                if (PlayerTeam[i].GetComponent<PlayerController>() != null)
                {


                    PlayerTeam[i].GetComponent<PlayerController>().enabled = false;
                    PlayerTeam[i].GetComponent<PlayerController>().SelectionRingHide();
                    PlayerTeam[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                    PlayerTeam[i].GetComponent<Animator>().SetFloat("Speed", PlayerTeam[i].GetComponent<Rigidbody>().velocity.magnitude);
                }
            }
        }

       
        if (isShotTaken ||Vector3.Distance(closestPlayer.transform.position, Ball.transform.position) < 8f || Ball.GetComponent<Ball>().GetPlayer() == null || Ball.GetComponent<Ball>().GetPlayer().GetComponent<PlayerController>().enabled == false)
        {
            if(closestPlayer.GetComponent<PlayerController>() != null)
            {
                Vector3.MoveTowards(closestPlayer.transform.position, Ball.transform.position, 100);
                Ball.GetComponent<Ball>().SetPlayerBallPosition(closestPlayer.GetComponent<PlayerController>().BallLocation);
                Ball.GetComponent<Ball>().SetPlayer(closestPlayer.transform);
                closestPlayer.GetComponent<PlayerController>().enabled = true;
                closestPlayer.GetComponent<PlayerController>().SelectionRingShow();

                closestPlayer.transform.position = Vector3.MoveTowards(closestPlayer.transform.position,Ball.transform.position,SROptions.Current.MoveSpeed*Time.deltaTime);

              
            }
        }

        

    }
    public GameObject PLayerToPass()
    {
        float bestOption = Mathf.Infinity;
        float fitness = Mathf.Infinity;
        GameObject passplayer = null;
       

        for (int i = 0; i < PlayerTeam.Length; i++)
        {

            if (PlayerTeam[i] != Ball.GetComponent<Ball>().GetPlayer())
            {
                if (Ball.GetComponent<Ball>().GetPlayer().transform.forward.x < 0) // YUKARISI
                {
                    
                
                    float directionofFace = Ball.GetComponent<Ball>().GetPlayer().transform.forward.x + Ball.GetComponent<Ball>().GetPlayer().transform.position.x;

                    if(PlayerTeam[i].transform.position.x< Ball.GetComponent<Ball>().GetPlayer().transform.position.x)
                    {
                        if (Ball.GetComponent<Ball>().GetPlayer().transform.forward.z < 0)
                        {
                            if(PlayerTeam[i].transform.position.z < Ball.GetComponent<Ball>().GetPlayer().transform.position.z)
                            {
                                //Debug.DrawLine(Ball.GetComponent<Ball>().GetPlayer().transform.position, PlayerTeam[i].transform.position,Color.blue,100000f);
                                // Debug.Log("Sol yukarı doğru oyuncular " + PlayerTeam[i]);
                                fitness = CalculateAngle(PlayerTeam[i]) * (CalculateDistance(PlayerTeam[i])/5);
                                 //Debug.Log(PlayerTeam[i] + ": Angle " + CalculateAngle(PlayerTeam[i]) + " Distance " + CalculateDistance(PlayerTeam[i]) + " Result : " + fitness);
                            }
                        }
                        else
                        {
                            if (PlayerTeam[i].transform.position.z > Ball.GetComponent<Ball>().GetPlayer().transform.position.z)
                            {
                               // Debug.DrawLine(Ball.GetComponent<Ball>().GetPlayer().transform.position, PlayerTeam[i].transform.position, Color.blue, 100000f);
                                // Debug.Log("Sağ yukarı doğru oyuncular " + PlayerTeam[i]);
                                fitness = CalculateAngle(PlayerTeam[i]) * (CalculateDistance(PlayerTeam[i])/5);
                                 // Debug.Log(PlayerTeam[i] + ": Angle " + CalculateAngle(PlayerTeam[i]) + " Distance " + CalculateDistance(PlayerTeam[i]) + " Result : " + fitness);
                            }
                        }
                    }
                }
                else//AŞŞAĞI
                {      
                    float directionofFace = Ball.GetComponent<Ball>().GetPlayer().transform.forward.x + Ball.GetComponent<Ball>().GetPlayer().transform.position.x;
                     if(PlayerTeam[i].transform.position.x > Ball.GetComponent<Ball>().GetPlayer().transform.position.x)
                    {
                        
                        if (Ball.GetComponent<Ball>().GetPlayer().transform.forward.z < 0)
                        {
                            if (PlayerTeam[i].transform.position.z < Ball.GetComponent<Ball>().GetPlayer().transform.position.z)
                            {
                                //Debug.DrawLine(Ball.GetComponent<Ball>().GetPlayer().transform.position, PlayerTeam[i].transform.position, Color.blue, 100000f);
                                //  Debug.Log("Sol Aşağı doğru oyuncular " + PlayerTeam[i]);
                                fitness = CalculateAngle(PlayerTeam[i]) * (CalculateDistance(PlayerTeam[i])/5);
                                // Debug.Log(PlayerTeam[i] + ": Angle " + CalculateAngle(PlayerTeam[i]) + " Distance " + CalculateDistance(PlayerTeam[i]) + " Result : " + fitness);
                            }
                        }
                        else
                        {
                            if (PlayerTeam[i].transform.position.z > Ball.GetComponent<Ball>().GetPlayer().transform.position.z)
                            {
                               // Debug.DrawLine(Ball.GetComponent<Ball>().GetPlayer().transform.position, PlayerTeam[i].transform.position, Color.blue, 100000f);
                                // Debug.Log("Sağ Aşağı doğru oyuncular " + PlayerTeam[i]);

                                fitness = CalculateAngle(PlayerTeam[i]) * (CalculateDistance(PlayerTeam[i])/5);
                              // Debug.Log(PlayerTeam[i] + ": Angle " + CalculateAngle(PlayerTeam[i]) + " Distance " + CalculateDistance(PlayerTeam[i]) + " Result : " + fitness);
                            }
                        }
                    }
                }
                if (fitness < bestOption)
                {
                  
                    bestOption = fitness;
                    passplayer = PlayerTeam[i];
                }
            }
            else
            {
                //Debug.DrawLine(PlayerTeam[i].transform.position, Ball.transform.position, Color.red, 100000);
            }
        }


        if(passplayer == null )
        {
            passplayer = SendRaycast();
        }
        else if(passplayer.CompareTag("GoalLine")){
            passplayer = SendRaycast();
        }
     
        
        return passplayer;
    }
    public float CalculateAngle(GameObject target)
    {
        Vector3 targetDir = target.transform.position - Ball.GetComponent<Ball>().GetPlayer().transform.position;
        Vector3 movedirection = Ball.GetComponent<Ball>().GetPlayer().transform.position - Ball.transform.position;
        float angle = Vector3.Angle(targetDir, movedirection);
        return angle / 2;

    }
    public float CalculateDistance(GameObject target)
    {
        return Vector3.Distance(target.transform.position,Ball.GetComponent<Ball>().GetPlayer().transform.position);
    }
    #region raycast
    public GameObject SendRaycast()
    {
        float closest = 1000f;
        closestPlayer = null;
        RaycastHit[] hits = Physics.RaycastAll(Ball.GetComponent<Ball>().GetPlayer().transform.position, Ball.GetComponent<Ball>().GetPlayer().transform.forward *20f,Mathf.Infinity);
        
        RaycastHit hitpoint = hits[0];
        foreach (var hit in hits)
        {
            if(!hit.collider.CompareTag("Arena") && !hit.collider.CompareTag("Ball") && !hit.collider.CompareTag("GoalLine")&& !hit.collider.CompareTag("Player"))
            {
                hitpoint = hit;
            }
            else if (hit.collider.CompareTag("GoalLine"))
            {
                closestPlayer = Goal;
            }
        }
        for (int i = 0; i < PlayerTeam.Length; i++)
        {
            if (PlayerTeam[i] != Ball.GetComponent<Ball>().GetPlayer() && closestPlayer != Goal)
            {
                float DistanceToBall = Vector3.Distance(PlayerTeam[i].transform.position, hitpoint.point);
                if (DistanceToBall < closest )
                {
                    closest = DistanceToBall;
                    closestPlayer = PlayerTeam[i];
                   
                }
            }
        }
        return closestPlayer;
    }
    #endregion
    void RestartGame()
    {
        ai.Gamestart = false;
        StartCoroutine(Ball.GetComponent<Ball>().Show());
        for (int i = 0; i < PlayerTeam.Length; i++)
        {
            if (PlayerTeam[i].GetComponent<PlayerController>() != null)
            {
                PlayerTeam[i].GetComponent<PlayerController>().enabled = true;
                PlayerTeam[i].GetComponent<PlayerController>().Restart();
                PlayerTeam[i].GetComponent<PlayerController>().enabled = false;
            }
        }
        Ball.SetActive(true);
        Time.timeScale = 1;
    }
    public void Score(GameObject goal)
    {
        if (goal.name == "Right")
        {
            AwayScore++;
            AwayScoreText.text = "AWAY : "  + AwayScore.ToString() ;
        }
        else
        {
            HomeScore++;
            HomeScoreText.text = "HOME : " + HomeScore.ToString();
        }
    }
    public void EndGame(int Home, int Away)
    {

        if (HomeScore > AwayScore)
        {
            FinalScore.text = HomeScore + " - " + AwayScore;
            ResultText.text = "You Won";
        }
        else if (AwayScore > HomeScore)
        {
            FinalScore.text = HomeScore + " - " + AwayScore;
            ResultText.text = "You Lost";
        }
        else
        {
            FinalScore.text = HomeScore + " - " + AwayScore;
            ResultText.text = "Draw";
        }
    }
    public void shoottakentrue()
    {
        isShotTaken = true;
    }
    public void shoottakenfalse()
    {
        isShotTaken = false;
    }
    public bool shottaken()
    {
        return isShotTaken;
    }
    public void SetPassPoint(Vector3 point)
    {
        PassPoint = point;
    }
    public Vector3 GetPassPoint()
    {

        return PassPoint;
    }
}
