using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    Vector3 lastCheckpoint;

    private void Start()
    {
        lastCheckpoint = transform.position;
        PlayerHealth.OnPlayerHurt += Respawn;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Checkpoint")
        {
            // Keep the player from reactivating an old checkpoint
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            // Update the last checkpoint
            lastCheckpoint = collision.gameObject.transform.position;
        }
    }

    // Respawn the player to the last checkpoint or at the start of the game
    private void Respawn()
    {
        transform.position = lastCheckpoint;
    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerHurt -= Respawn;
    }
}
