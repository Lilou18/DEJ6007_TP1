using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] Transform[] platformDestinations;
    private int index;
    private Transform target;
    [SerializeField] float platformSpeed;

    private void Start()
    {
        target = platformDestinations[0];
        index = 0;
    }

    private void Update()
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
}
