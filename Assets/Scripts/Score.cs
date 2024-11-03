using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Score : MonoBehaviour
{
    // This class calculates the scores of the player at the end of the level

    private int totalScore;

    private FriendManager friendManager;
    private PlayerHealth playerHealth;
    private PlayerCollection playerCollection;

    [SerializeField] private GameObject scorePanel;

    public static event Action<int> OnSetScore; // Invoke event to display the score

    private void Start()
    {
        friendManager = GetComponent<FriendManager>();
        playerHealth = GetComponent<PlayerHealth>();
        playerCollection = GetComponent<PlayerCollection>();
        PlayerCollision.OnGameEnd += SetScore;
    }

    public void SetScore()
    {
        StartCoroutine("WaitForScorePanel");       
    }

    // We wait until the Game Manager has activated the score panel then we calculate the score and call
    // for the event to display it.
    private IEnumerator WaitForScorePanel()
    {
        yield return new WaitUntil(() => scorePanel.activeInHierarchy);
        totalScore = (friendManager.NumberFriends * 5) + (playerHealth.health * 5) + (playerCollection.NumberSouls * 2);
        
        if (OnSetScore != null)
        {
            OnSetScore.Invoke(totalScore); // Event to display the score
        }
    }

    private void OnDisable()
    {
        PlayerCollision.OnGameEnd -= SetScore;
    }
}
