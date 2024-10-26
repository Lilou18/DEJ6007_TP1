using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int health { get; private set;}
    [SerializeField] GameObject[] heart;

    public static event Action OnPlayerDied;

    private void Start()
    {
        health = 3;
    }
    public void TakeDamage()
    {
        health--;
        SetHearts();
        // The player is dead
        if(health <= 0)
        {
            OnPlayerDied.Invoke();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage();
        }
    }
}
