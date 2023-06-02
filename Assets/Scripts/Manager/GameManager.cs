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
    [SerializeField] BaseStateMachine[] AIReset;
   

    [Header("Game")]
    [SerializeField] GameObject Ball;
    [SerializeField] GameObject[] PlayerTeam;
    [SerializeField] GameObject[] EnemyTeam;
    [SerializeField] GameObject Goal;
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] Transform BallSpawnPoint;
    [SerializeField] float RespawnTimer = 3.5f;

    private bool canTackle = true;
    private float tackleTimer=3f;
    private float TackleDistance = 10f;
    private bool BallOnEnemy;
    private GameObject closestPlayer;
    private GameObject closestEnemy;
    private int HomeScore = 0;
    private int AwayScore = 0;
    private float GameTime = 90000f;
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
        if (tackleTimer <= 0)
        {
            canTackle = true;
        }
        else
        {
            tackleTimer -= Time.deltaTime;
        }
        
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
       
        if(Ball.GetComponent<Ball>().GetPlayer() != null)
        {
            
            if (Ball.GetComponent<Ball>().GetPlayer().GetComponent<Enemy>() != null)
            {
                BallOnEnemy = true;
            }
            else
            {
                BallOnEnemy = false;
            }
        }
        else
        {
            BallOnEnemy = false;
        }
            CheckTheClosestPlayer();  
    }



    void CheckTheClosestPlayer()
    {
        if (isShotTaken)
        {
            Vector3.MoveTowards(PLayerToPass().transform.position, Ball.transform.position, 100);
            PLayerToPass().transform.position = Vector3.MoveTowards(PLayerToPass().transform.position, Ball.transform.position, SROptions.Current.MoveSpeed * Time.deltaTime);
        }

        if (BallOnEnemy && Ball.GetComponent<Ball>().GetPlayer()!= null)
        {
            if (Vector3.Distance(Ball.GetComponent<Ball>().GetPlayer().transform.position, closestPlayer.transform.position) < TackleDistance && canTackle)
            {
                Debug.Log("tackles");
                canTackle = false;
                tackleTimer = 2f;
                closestPlayer.GetComponent<PlayerController>().Tackle(Ball.GetComponent<Ball>().GetPlayer());
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
                if (Ball.GetComponent<Ball>().GetPlayer().transform.forward.x < 0) // YUKARI
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

                                fitness = CalculateAngle(PlayerTeam[i]) * (CalculateDistance(PlayerTeam[i])/10);
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
        for (int i = 0; i < AIReset.Length; i++)
        {
            AIReset[i].Gamestart = false;
        }
        
 
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
        for (int i = 0; i < EnemyTeam.Length; i++)
        {
            if (EnemyTeam[i].GetComponent<Enemy>() != null)
            {
                
                EnemyTeam[i].GetComponent<Enemy>().Restart();
        
            }
        }
        Ball.SetActive(true);
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
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
}
