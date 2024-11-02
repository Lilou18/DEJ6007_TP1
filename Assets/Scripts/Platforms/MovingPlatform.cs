using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] Transform[] platformDestinations;
    private int index;
    private Transform target;
    [SerializeField] float platformSpeed;

    GameObject player;
    Transform initParent;

    private void Start()
    {
        target = platformDestinations[0];
        index = 0;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            target = platformDestinations[index];
            index = NewPlatformDestinationIndex();
        }
        else
        {
            this.transform.position = Vector2.MoveTowards(transform.position, target.position, platformSpeed * Time.deltaTime);
        }
    }

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
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            initParent = player.transform.parent;
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && this.gameObject.activeInHierarchy)
        {
            collision.gameObject.transform.parent = initParent;
        }
    }
}
