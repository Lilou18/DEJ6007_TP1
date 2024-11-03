using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushingBlock : MonoBehaviour
{
    // This class manages the movement of the crushing blocks

    [SerializeField] private float waitTimer; // Time we wait before the block rises again
    [SerializeField] private float fallSpeed; // Speed of the block when it falls
    [SerializeField] private float upSpeed;   // Speed of the block when it rises

    private Vector3 initialPosition;
    [SerializeField] private Transform fallingPosition;
    [SerializeField] private float fallingTimer;  // Create a delay at the start so the crushing blocks dont fall at the same time
    [SerializeField] private float fallingCooldown;

    private bool canFall;
    private bool isWaiting;
    private void Start()
    {        
        initialPosition = transform.position;
        canFall = true;
    }
    private void Update()
    {
        if (canFall)
        {
            // Check if the delay has pass
            fallingTimer += Time.deltaTime;
            if (fallingTimer >= fallingCooldown)
            {
                Fall();
            }
        }
        else if(!canFall && !isWaiting)
        {
            Rise();
        }
    }

    private void Fall()
    {
        // Make the block fall to the falling position
        transform.position = Vector2.MoveTowards(transform.position, fallingPosition.position, fallSpeed * Time.deltaTime);

        // Check if the block has reached the falling position
        if (Vector2.Distance(transform.position, fallingPosition.position) < 0.1f)
        {
            canFall = false;
            fallingTimer = 0f;  
            StartCoroutine(StartRising());
        }
    }

    // Rise the block back to it's initial position
    private void Rise()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, upSpeed * Time.deltaTime);

        // Check if the block has reached the inital position
        if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
        {
            canFall = true;  
            fallingTimer = 0f;
        }
    }

    // We wait a few seconds before the block rise again
    private IEnumerator StartRising()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimer);
        // As long as the block rise it can't fall
        canFall = false; 
        isWaiting = false;
    }
}
