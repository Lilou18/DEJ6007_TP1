using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    // This class manage many collisions with the player

    private PlayerHealth playerHealth;
    Animator animator;

    public static event Action OnGameEnd; // Invoke the event when the game end
        

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponentInChildren<Animator>();        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player falls then we make him repsawn and loose a life
        if(collision.gameObject.tag == "Fall")
        {
            playerHealth.TakeDamage(1);

            // Disable camera follow when falling
            Camera.main.gameObject.GetComponent<CameraMovement>().enabled = false;
        }        
        else if(collision.gameObject.tag == "CrushingBlock") // If the player touch the crushing block he loose a life
        {
            playerHealth.TakeDamage(1);
        }
        else if(collision.gameObject.tag == "End") // The player arrived at the portal which is the ending
        {
            SoundManager.Instance.PlaySound("Ending");
            animator.SetBool("IsTeleporting", true);
            StartCoroutine("WaitTeleportingAnimation");
        }        
    }
    // We wait until the portal animation si finished before showing the score panel
    private IEnumerator WaitTeleportingAnimation()
    {
        yield return new WaitForSeconds(1f);
        OnGameEnd.Invoke();
    }

    // If the player enter in collision with an enemy then he takes damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            playerHealth.TakeDamage(1);
        }
        else if (collision.gameObject.tag == "FireBall") // If the player enter in collision with a fireball then he takes damage
        {
            playerHealth.TakeDamage(1);
        }
    }

}
