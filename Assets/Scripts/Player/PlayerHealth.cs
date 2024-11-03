using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Manage the health of the player
    [SerializeField] public int health { get; private set;}
    [SerializeField] GameObject[] heart; // UI that represent the life of the player

    public static event Action OnPlayerDied; // Invoke this event when the player dies

    public static event Action OnPlayerHurt; // Invoke this event when the player get hurt

    private bool isInvincible; // Check if the player is invincible

    private void Start()
    {
        health = 3;
        isInvincible = false;
    }
    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            health -= damage;
            SetHearts(); // Update the number of life of the player in the UI

            // The player is dead
            if (health <= 0)
            {
                OnPlayerDied.Invoke();
            }
            else
            {
                // The player is hurt
                OnPlayerHurt.Invoke();
            }
            // When the player gets hurt he becomes invincible for 1 second
            StartCoroutine(Invincible());
        }
        
    }

    // Set the number of hearts to be displayed correctly on the Canvas
    public void SetHearts()
    {
        for(int i = 0; i < heart.Length; i++)
        {
            if(i < health)
            {
                heart[i].SetActive(true);
            }
            else
            {
                heart[i].SetActive(false);
            }
        }
    }

    // When the player gets hurt we want to make him invincible for 1 secund to make sure he doesn't take more damage before
    // he respawns to the checkpoint or his initial position
    private IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }
}
