using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    // This class control high-level game events

    [SerializeField] GameObject deadPanel;  // Panel to display when the player dies
    [SerializeField] GameObject scorePanel; // Panel to display when the player finish the game

    private void Start()
    {
        PlayerHealth.OnPlayerDied += PlayerDie;
        PlayerCollision.OnGameEnd += FinishLevel;
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            Application.Quit();
        }
    }

    // Display the panel that propose to the player to play again
    public void PlayerDie()
    {
        deadPanel.SetActive(true);
        Time.timeScale = 0f;    // Pause the game when the player dies
    }

    // Restard the level and unpause the game
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    // Display the score panel when the player wins
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
