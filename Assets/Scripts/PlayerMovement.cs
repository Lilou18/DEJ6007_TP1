using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }
     
    void Update()
    {
        
        if(Input.GetAxis("Horizontal") != 0)
        {
            if(Input.GetAxis("Horizontal") > 0) // We run to the right
            {
                animator.SetBool("IsWalkingRight", true);
            }
            else if(Input.GetAxis("Horizontal") < 0) // We run to the left
            {
                animator.SetBool("IsWalkingLeft", true);
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
}
