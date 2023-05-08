using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    
  
    [Header("Game")]
    [SerializeField] GameObject Ball;
    [SerializeField] GameObject[] PlayerTeam;
    
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] Transform BallSpawnPoint;

    [SerializeField] float RespawnTimer = 3.5f;
    GameObject closestPlayer;
    private int HomeScore = 0;
    private int AwayScore = 0;
    private float GameTime = 90f;
    private bool isRestart;

   

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
            RestartGame();
            Time.timeScale = 0f;
            EndGame(HomeScore, AwayScore);
            GameOverScreen.SetActive(true);
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
            if (PlayerTeam[i] != closestPlayer)
            {
                PlayerTeam[i].GetComponent<PlayerController>().enabled = false;
                PlayerTeam[i].GetComponent<PlayerController>().SelectionRingHide();
                PlayerTeam[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                PlayerTeam[i].GetComponent<Animator>().SetFloat("Speed", PlayerTeam[i].GetComponent<Rigidbody>().velocity.magnitude);
            }
        }
        
        Ball.GetComponent<Ball>().SetPlayerBallPosition(closestPlayer.GetComponent<PlayerController>().BallLocation);
        Ball.GetComponent<Ball>().SetPlayer(closestPlayer.transform);
        closestPlayer.GetComponent<PlayerController>().enabled = true ;
        closestPlayer.GetComponent<PlayerController>().SelectionRingShow();
    }
    void RestartGame()
    {

        StartCoroutine(Ball.GetComponent<Ball>().Show());
        for (int i = 0; i < PlayerTeam.Length; i++)
        {
            PlayerTeam[i].GetComponent<PlayerController>().enabled = true;
            PlayerTeam[i].GetComponent<PlayerController>().Restart();
            PlayerTeam[i].GetComponent<PlayerController>().enabled = false;
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
}
