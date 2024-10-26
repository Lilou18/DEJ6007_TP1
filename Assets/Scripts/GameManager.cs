using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject deadPanel;

    private void Start()
    {
        PlayerHealth.OnPlayerDied += playerDie;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= playerDie;
    }

    public void playerDie()
    {
        deadPanel.SetActive(true);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
