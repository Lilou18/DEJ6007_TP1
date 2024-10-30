using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Friend : MonoBehaviour, ICollectable
{
    [SerializeField] private Transform followed;
    [SerializeField] public Transform Followed { get{return followed;} set{ if (value != null) { followed = value; }} }
    [SerializeField] float speed;
    [SerializeField] GameObject friendMarker;
    public GameObject FriendMarker { get { return friendMarker; } }
    private bool markerRight; // Is the marker is on the right side of the friend

    private Animator animator;
    private Vector3 currentVelocity = Vector3.zero; // Vitesse pour SmoothDamp

    private void Start()
    {
        markerRight = true;
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {

        if (followed != null)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput > 0 && markerRight) // The friend is mooving to the right and the marker is on the right side
            {
                FlipMarkerFriend();
            }
            else if (horizontalInput < 0 && !markerRight) //// The friend is mooving to the left and the marker is on the left side
            {
                FlipMarkerFriend();
            }

            if (Vector3.Distance(transform.position, followed.position) > 0.1f) // L'ami est encore en mouvement
            {
                animator.SetBool("IsWalkingRight", transform.position.x < followed.position.x);
                animator.SetBool("IsWalkingLeft", transform.position.x > followed.position.x);
            }
            else // L'ami est arrivé à la position cible
            {
                animator.SetBool("IsWalkingLeft", false);
                animator.SetBool("IsWalkingRight", false);
            }
            transform.position = Vector3.Lerp(transform.position, followed.position, Time.deltaTime * speed);
        }
    }

    private void LateUpdate()
    {

    }

    // We change the marker to be on the other side of the friend, to follow the player movement
    private void FlipMarkerFriend()
    {
        markerRight = !markerRight;
        Vector3 friendMarkerNewPosition = friendMarker.transform.localPosition;
        friendMarkerNewPosition.x = -friendMarkerNewPosition.x;
        friendMarker.transform.localPosition = friendMarkerNewPosition;
    }

    public void Collect()
    {

    }

}
