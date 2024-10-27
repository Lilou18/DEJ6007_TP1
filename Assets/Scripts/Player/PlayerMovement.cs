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

    [SerializeField] GameObject friendMarker;
    bool markerRight = true;

    //private bool canDash;
    //private bool isDashing;
    //private float dashingPower = 24f;
    //private float dashingTime = 0.2f;
    //private float dashingCooldown = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();

        trailRenderer = GetComponent<TrailRenderer>();
        

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

        if (IsGrounded())
        {
            doubleJump = true;
        }
        if(IsGrounded() && waitCoolDown)
        {
            canDash = true;
            waitCoolDown = false;
        }
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                rb.velocity = Vector2.up * jumpForce; // Modifier gravity scale                
            }
            else if (Input.GetKeyDown(KeyCode.Space) && doubleJump){
                rb.velocity = Vector2.up * jumpForce;
                doubleJump = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            isDashing = true;
            canDash = false;
            trailRenderer.emitting = true;
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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
    private bool IsGrounded()
    {
        float distance = circleCollider2D.bounds.extents.y + distanceDelta;
        Debug.DrawRay(transform.position, Vector2.down * distance, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance, platformsLayerMask);
        return hit.collider != null;
        
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        trailRenderer.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        waitCoolDown = true;
    }

    //private IEnumerator Dash()
    //{
    //    print("Hello!");
    //    canDash = false;
    //    isDashing = true;
    //    float originalGravity = rb.gravityScale;
    //    rb.gravityScale = 0f;
    //    rb.velocity = new Vector2(Input.GetAxis("Horizontal") * dashingPower, 0f);
    //    yield return new WaitForSeconds(dashingTime);
    //    rb.gravityScale = originalGravity;
    //    isDashing = false;
    //    yield return new WaitForSeconds(dashingCooldown);
    //    canDash = true;
    //}
}
