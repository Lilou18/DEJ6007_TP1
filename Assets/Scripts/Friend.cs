using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Friend : MonoBehaviour
{ 
    // This class manage the behaviour of the friends

    [SerializeField] private Transform followed; // Transform that the friend must follow
    [SerializeField] public Transform Followed { get{return followed;} set{ if (value != null) { followed = value; }} }
    [SerializeField] float speed;
    [SerializeField] GameObject friendMarker; // This his the GameObject attached to the friend, that the new friend will follow
    public GameObject FriendMarker { get { return friendMarker; } }
    private bool markerRight; // Is the marker on the right side of the friend

    private Animator animator;

    private void Start()
    {
        markerRight = true;
        animator = GetComponentInChildren<Animator>();
    }
   
    private void FixedUpdate()
    {
        if (followed != null)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            // If the player is mooving to the right and the marker is on the right side
            // We change the position of the friend marker to be on the left so the friends that follow
            // will look like they are following him
            if (horizontalInput > 0 && markerRight)
            {
                FlipMarkerFriend();
            }
            // If the player is mooving to the left and the marker is on the left side
            // We change the position of the friend marker to be on the right so the friends that follow
            // will look like they are following him
            else if (horizontalInput < 0 && !markerRight)
            {
                FlipMarkerFriend();
            }

            // If the friend is still moving to join the player or the other friends then we continue his walking animation
            if (Vector3.Distance(transform.position, followed.position) > 0.1f)
            {
                animator.SetBool("IsWalkingRight", transform.position.x < followed.position.x);
                animator.SetBool("IsWalkingLeft", transform.position.x > followed.position.x);
            }
            else // The friend has joined the player or the other friends
            {
                animator.SetBool("IsWalkingLeft", false);
                animator.SetBool("IsWalkingRight", false);
            }
            transform.position = Vector3.Lerp(transform.position, followed.position, Time.deltaTime * speed);
        }
    }

    // We change the marker to be on the other side of the friend, to follow the player movement
    private void FlipMarkerFriend()
    {
        markerRight = !markerRight;
        Vector3 friendMarkerNewPosition = friendMarker.transform.localPosition;
        friendMarkerNewPosition.x = -friendMarkerNewPosition.x;
        friendMarker.transform.localPosition = friendMarkerNewPosition;
    }
}
