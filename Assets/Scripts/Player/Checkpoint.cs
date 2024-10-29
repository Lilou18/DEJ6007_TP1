using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Vector3 lastCheckpoint; // Last checkpoint of the player

    private void Start()
    {
        lastCheckpoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            lastCheckpoint = collision.transform.position; // We update our checkpoint
            collision.gameObject.GetComponent<Collider>().enabled = false; // Keep the player from reactivating an old checkpoint
        }
    }

    public void Respawn()
    {
        transform.position = lastCheckpoint;
    }
}
