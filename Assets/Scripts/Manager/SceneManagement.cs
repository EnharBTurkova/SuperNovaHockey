using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] GameObject Controller;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject PauseButton;

    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
    
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void Pause()
    {
        Controller.SetActive(false);
        PauseButton.SetActive(false);
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;

    }
    public void Continue()
    {
        Time.timeScale = 1f;
        Controller.SetActive(true);
        PauseButton.SetActive(true);
        PauseMenu.SetActive(false);
       

    }
}
