using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int health;

    private void Start()
    {
        health = 2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "FireBallPlayer")
        {
            health--;
            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
