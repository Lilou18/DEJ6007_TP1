using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    private PlayerHealth playerHealth;

    public static event Action OnGameEnd;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Fall")
        {
            playerHealth.TakeDamage(1);

            // Keep the camera from following the player when he falls into a hole
            Camera.main.gameObject.GetComponent<CameraMovement>().enabled = false;
        }
        else if(collision.gameObject.tag == "FireBall")
        {
            playerHealth.TakeDamage(1);
            Destroy(collision.gameObject);
            //SoundManager.Instance.PlaySound(SoundManager.Instance.Test);
        }
        else if(collision.gameObject.tag == "CrushingBlock")
        {
            playerHealth.TakeDamage(1);
        }
        else if(collision.gameObject.tag == "End")
        {
            OnGameEnd.Invoke();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            playerHealth.TakeDamage(1);
        }
    }

}
