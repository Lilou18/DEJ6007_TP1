using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] GameObject[] heart;

    private void Start()
    {
        health = 3;
    }
    public void TakeDamage()
    {
        health--;
        if(health >= 0)
        {
            // Take Damage
            heart[health].SetActive(false);
        }
        else
        {
            health = 0;
            // Player die!
        }
    }
}
