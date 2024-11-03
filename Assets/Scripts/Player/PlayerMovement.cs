using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // This class manage the movement of the player

    public float speed;
    public float jumpForce;

    public Rigidbody2D rb;

    CircleCollider2D circleCollider2D;
    [SerializeField]private LayerMask platformsLayerMask; // We verify if the player is touching something with this layer
    private bool doubleJump; // Can the player double jump

    public Animator animator;

    public float distanceDelta; // Offset to make sure the player his touching the ground

    // Dash variables
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float dashVelocity;
    [SerializeField] private float dashTime;
    [SerializeField]private float dashCoolDown;
    private Vector2 dashDirection;
    private bool isDashing;
    private bool canDash;
    private bool waitCoolDown;

    [SerializeField] GameObject friendMarker; // Gameobject near the player that the friends are following
    bool markerRight = true; // Is the marker on the right side of the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        canDash = true;
    }
     
    void Update()
    {
        
        if(Input.GetAxis("Horizontal") != 0)
        {
          
            if (Input.GetAxis("Horizontal") > 0) // We run to the right
            {
                animator.SetBool("IsWalkingRight", true);                
                if (markerRight)
                {
                    FlipMarkerFriend(); // If we run to the right then we want the friends to follow on the left side of the player
                }
            }
            else if(Input.GetAxis("Horizontal") < 0) // We run to the left
            {
                animator.SetBool("IsWalkingLeft", true);
                if (!markerRight)
                {                    
                    FlipMarkerFriend(); // If we run to the left then we want the friends to follow on the right side of the player
                }
            }
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);

        }
        else
        {
            animator.SetBool("IsWalkingLeft", false);
            animator.SetBool("IsWalkingRight", false);
        }

        // Check if the player can dash
        if(IsGrounded() && waitCoolDown)
        {
            canDash = true;
            waitCoolDown = false;
        }
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // If the player is on the ground then he can jump
            if (IsGrounded())
            {
                rb.velocity = Vector2.up * jumpForce; // Modifier gravity scale
                doubleJump =true;
            }
            // If the player as not already double jump then he can double jump
            else if ( doubleJump){
                rb.velocity = Vector2.up * jumpForce;
                doubleJump = false;
            }
        }
        // Check if the player can dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            isDashing = true;
            canDash = false;
            trailRenderer.emitting = true;

            // The player control the dash direction
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // Set a default direction if the player hit the dash button
            if(dashDirection == Vector2.zero)
            {
                dashDirection = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(StopDashing());
        }

        if (isDashing)
        {
            rb.velocity = dashDirection.normalized * dashVelocity;
            return;
        }
    }
    // We change the position of the friend marker so the friends look like they are following the same direction has the player
    private void FlipMarkerFriend()
    {
        markerRight = !markerRight;
        Vector3 friendMarkerNewPosition = friendMarker.transform.localPosition;
        friendMarkerNewPosition.x = -friendMarkerNewPosition.x;
        friendMarker.transform.localPosition = friendMarkerNewPosition;
    }

    // Inspire for jump, double jump and dash
    // https://www.youtube.com/watch?v=ptvK4Fp5vRY
    //https://www.youtube.com/watch?v=DEGEEZmfTT0
    //https://www.youtube.com/watch?v=9pKXXNgCgq8

    // Make sure the player is on the ground before he can jump
    private bool IsGrounded()
    {
        float distance = circleCollider2D.bounds.extents.y + distanceDelta;
        Vector2 origin = (Vector2)transform.position - new Vector2(0, circleCollider2D.bounds.extents.y);

        // We make sure if a single part of our player is on the platform (or the ground). If it his then he's considered grounded
        RaycastHit2D hitCenter = Physics2D.Raycast(origin, Vector2.down, distance, platformsLayerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin + new Vector2(-circleCollider2D.bounds.extents.x, 0), Vector2.down, distance, platformsLayerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(origin + new Vector2(circleCollider2D.bounds.extents.x, 0), Vector2.down, distance, platformsLayerMask);

        return hitCenter.collider != null || hitLeft.collider != null || hitRight.collider != null;

    }

    // Cooldown after the player dashed
    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime); // We wait until the dash is finished
        trailRenderer.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown); // We wait until the cooldown between dashes his finished
        // Allow the player to dash again
        waitCoolDown = true;
    }
}
