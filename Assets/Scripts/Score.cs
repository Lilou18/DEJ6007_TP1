using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Score : MonoBehaviour
{
    private int totalScore;

    private FriendManager friendManager;
    private PlayerHealth playerHealth;
    private PlayerCollection playerCollection;

    [SerializeField] private GameObject scorePanel;

    public static event Action<int> OnSetScore;

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

    private IEnumerator WaitForScorePanel()
    {
        yield return new WaitUntil(() => scorePanel.activeInHierarchy);
        totalScore = (friendManager.NumberFriends * 5) + (playerHealth.health * 5) + (playerCollection.NumberSouls * 2);
        print(friendManager.NumberFriends);
        print(playerHealth.health);
        print(playerCollection.NumberSouls);
        
        if (OnSetScore != null)
        {
            OnSetScore.Invoke(totalScore);
        }
    }

    private void OnDisable()
    {
        PlayerCollision.OnGameEnd -= SetScore;
    }
}
