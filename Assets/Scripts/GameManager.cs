using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject deadPanel;

    private void Start()
    {
        PlayerHealth.OnPlayerDied += PlayerDie;
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
    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= PlayerDie;
    }
}
