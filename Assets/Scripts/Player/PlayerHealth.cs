using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int health { get; private set;}
    [SerializeField] GameObject[] heart;
    private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Sprite playerHurt;

    public static event Action OnPlayerDied;
    public static event Action OnPlayerHurt;

    private void Start()
    {
        health = 3;
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        SetHearts();
        //StartCoroutine(Hurt());
        // The player is dead
        if(health <= 0)
        {
            OnPlayerDied.Invoke();
        }
        else
        {
            OnPlayerHurt.Invoke();
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

    // Change the color of the spirte when the player is hurt
    //IEnumerator Hurt()
    //{
    //    Sprite initSprite = playerSpriteRenderer.sprite;
    //    playerSpriteRenderer.sprite = playerHurt;
    //    yield return new WaitForSeconds(0.2f);
    //    playerSpriteRenderer.sprite = initSprite;
    //}

}
