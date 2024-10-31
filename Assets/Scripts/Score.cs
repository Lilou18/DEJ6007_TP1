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
        GameManager.OnLevelFinished += SetScore;
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
        print(totalScore);

        if (OnSetScore != null)
        {
            OnSetScore.Invoke(totalScore);
            print("On a invoke");
        }
    }

    private void OnDisable()
    {
        GameManager.OnLevelFinished -= SetScore;
    }
}
