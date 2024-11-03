using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // This class manage the behaviour of a platform that move between multiple patrol points.

    [SerializeField] Transform[] platformDestinations; // Destinations the platofrm must reach
    private int index;
    private Transform target; // Next destination the enemy must go to
    [SerializeField] float platformSpeed;

    GameObject player;
    Transform initParent; // The parent of the player

    private void Start()
    {
        target = platformDestinations[0];
        index = 0;
    }

    private void FixedUpdate()
    {
        // If the platform reaches it's destination, we give it a new destination
        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            target = platformDestinations[index];
            index = NewPlatformDestinationIndex();
        }
        else
        { 
            // The platform moves towards it's destination
            this.transform.position = Vector2.MoveTowards(transform.position, target.position, platformSpeed * Time.deltaTime);
        }
    }

    // Set the new destination of the platform
    private int NewPlatformDestinationIndex()
    {
        index++;
        if(index >= platformDestinations.Length)
        {
            index = 0;
        }
        return index;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player jumps on the platform we must change his parent so he can follow the movement of the platform
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            initParent = player.transform.parent;
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // When the player leaves the platform we reset his parent
        if (collision.gameObject.tag == "Player" && this.gameObject.activeInHierarchy)
        {
            collision.gameObject.transform.parent = initParent;
        }
    }
}
