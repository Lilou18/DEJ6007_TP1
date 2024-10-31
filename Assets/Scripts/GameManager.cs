using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject deadPanel;
    [SerializeField] GameObject scorePanel;

    public static event Action OnLevelFinished;

    private void Start()
    {
        PlayerHealth.OnPlayerDied += PlayerDie;
        PlayerCollision.OnGameEnd += FinishLevel;
    }    

    public void PlayerDie()
    {
        deadPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void FinishLevel()
    {
        scorePanel.SetActive(true);
        //ScoreUI scoreUI = scorePanel.GetComponent<ScoreUI>();
       // if(scoreUI != null)
        //{
            //OnLevelFinished.Invoke();
            //Score.OnSetScore += scoreUI.UpdateScoreText;
        //}
        OnLevelFinished.Invoke();
    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= PlayerDie;
        PlayerCollision.OnGameEnd -= FinishLevel;
    }
}
