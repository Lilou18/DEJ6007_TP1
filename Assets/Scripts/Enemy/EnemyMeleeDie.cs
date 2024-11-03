using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeDie : MonoBehaviour
{
    // This script is used for the collider that's on the head of the enemy melee.

    EnemyHealth health;

    private void Start()
    {
        health = GetComponentInParent<EnemyHealth>();
    }
    
    // We want the enemy to die when a fireball or the player hits him on the head.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "FireBallPlayer")
        {
            health.TakeDamage(1);
        }
    }

}
