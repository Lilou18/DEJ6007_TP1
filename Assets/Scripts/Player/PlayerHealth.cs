using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int health { get; private set;}
    [SerializeField] GameObject[] heart;

    public static event Action OnPlayerDied;

    public static event Action OnPlayerHurt;

    private bool isInvincible;

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
            SetHearts();
            // The player is dead
            if (health <= 0)
            {
                OnPlayerDied.Invoke();
            }
            else
            {
                OnPlayerHurt.Invoke();
            }
            StartCoroutine(Invincible());
        }
        
    }

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

    private IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }
}
