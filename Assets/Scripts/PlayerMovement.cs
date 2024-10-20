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

    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //boxCollider2D = GetComponent<BoxCollider2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
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
        else
        {
           animator.SetBool("IsJumpingStart", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                rb.velocity = Vector2.up * jumpForce; // Modifier gravity scale
                animator.SetBool("IsJumpingStart",true);
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
        //RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        //RaycastHit2D hit = Physics2D.CircleCast(circleCollider2D.bounds.center, circleCollider2D.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        RaycastHit2D hit = Physics2D.CircleCast(circleCollider2D.bounds.center, 1.28f, Vector2.down, 0.1f, platformsLayerMask);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
