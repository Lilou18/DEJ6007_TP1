using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform targetPlayer;
    Vector3 offset; // Distance between camera and the player
    public float camMoveSpeed;
    
    void Start()
    {
        offset = transform.position - targetPlayer.position;
    }
    private void FixedUpdate()
    {
        Vector3 newCameraPosition = targetPlayer.position + offset;
        transform.position = Vector3.Lerp(transform.position, newCameraPosition, Time.deltaTime * camMoveSpeed);
    }

    //https://www.youtube.com/watch?v=uXwmX92MALc
}
