using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    public Rigidbody2D rb;
    Vector2 movementDir;

    CircleCollider2D circleCollider2D;
    [SerializeField]private LayerMask platformsLayerMask;
    private bool doubleJump;

    public Animator animator;

    public float distanceDelta;

    // Dash variables
    private TrailRenderer trailRenderer;
    [SerializeField] private float dashVelocity;
    [SerializeField] private float dashTime;
    [SerializeField]private float dashCoolDown;
    private Vector2 dashDirection;
    private bool isDashing;
    private bool canDash;
    private bool waitCoolDown;

    [SerializeField] GameObject friendMarker; // Gameobject near the player that the friends are following
    bool markerRight = true; // Is the marker on the right side of the player

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }
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
                    FlipMarkerFriend();
                }
            }
            else if(Input.GetAxis("Horizontal") < 0) // We run to the left
            {
                animator.SetBool("IsWalkingLeft", true);
                if (!markerRight)
                {                    
                    FlipMarkerFriend();
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

    // https://www.youtube.com/watch?v=ptvK4Fp5vRY
    //https://www.youtube.com/watch?v=DEGEEZmfTT0
    //https://www.youtube.com/watch?v=9pKXXNgCgq8
    // Make sure the player is on the ground before he can jump
    private bool IsGrounded()
    {
        float distance = circleCollider2D.bounds.extents.y + distanceDelta;
        Vector2 origin = (Vector2)transform.position - new Vector2(0, circleCollider2D.bounds.extents.y);

        // We make sure if a single part of our player is on the platform
        RaycastHit2D hitCenter = Physics2D.Raycast(origin, Vector2.down, distance, platformsLayerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin + new Vector2(-circleCollider2D.bounds.extents.x, 0), Vector2.down, distance, platformsLayerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(origin + new Vector2(circleCollider2D.bounds.extents.x, 0), Vector2.down, distance, platformsLayerMask);

        return hitCenter.collider != null || hitLeft.collider != null || hitRight.collider != null;

    }

    // Cooldown after the player dashed
    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        trailRenderer.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        waitCoolDown = true;
    }
}
