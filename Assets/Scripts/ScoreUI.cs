using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TMP_Text scoreText;
    [SerializeField] private GameObject scorePanel;


    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        Score.OnSetScore += UpdateScoreText;
    }

    public void UpdateScoreText(int score)
    {
        print("Le score est " + score.ToString());
        scorePanel.SetActive(true);
        scoreText.text = "Score: " + score.ToString();
    }

    private void OnDisable()
    {
        Score.OnSetScore -= UpdateScoreText;
    }
}
