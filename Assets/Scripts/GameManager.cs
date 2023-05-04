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
  
    [Header("Game")]
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject Ball;
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] Transform BallSpawnPoint;

    [SerializeField] float RespawnTimer = 3.5f;
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

    }


    void RestartGame()
    {

        StartCoroutine(Ball.GetComponent<Ball>().Show());
        
      
        
        //Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
       // Player.GetComponent<Rigidbody2D>().rotation = 0;
        //Enemy.GetComponent<EnemyAI>().Attack();
   
        Ball.SetActive(true);
        Time.timeScale = 1;

    }


    public void Score(GameObject goal)
    {
        
        Debug.Log("asd");
        if (goal.name == "Right")
        {
            Debug.Log("asddwq");
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
            FinalScore.text = "You Win " + Home + " - " + Away;
        }
        else if (AwayScore < HomeScore)
        {
            FinalScore.text = "You Lose " + Home + " - " + Away;
        }
        else
        {
            FinalScore.text = "Draw " + Home + " - " + Away;
        }
    }
}
