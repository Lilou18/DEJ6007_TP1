using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject deadPanel;
    [SerializeField] GameObject scorePanel;

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
    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= PlayerDie;
        PlayerCollision.OnGameEnd -= FinishLevel;
    }
}
