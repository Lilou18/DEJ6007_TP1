using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    // This class manage the behaviour of the melee enemy

    [SerializeField] private float detectDistance;  // Distance where he can detect the player
    [SerializeField] private LayerMask layerMask;   // The layer we dont want the ray to collide with
                                                    // Here it's the collider of the enemy
    [SerializeField] Animator animator;

    PlayerHealth playerHealth;
    private bool canChase;
    private bool canAttack;

    [SerializeField] GameObject exclamation;    // Picture that shows to the player he's been spotted

    [SerializeField] Transform target;          // The player the enemy must attack
    [SerializeField] float enemySpeed;
    
    private bool playerIsVisible;   // Can the enemy see the player

    EnemyPatrol patrol; // Allow the enemy to patrol between multiple points
    EyeControl[] eyeControl; // Allow the eyes of the enemy to follow the player

    EnemyMeleeAttack enemyAttack; // Damage the player
    EnemyHealth health;    

    void Start()
    {
        canAttack = true;
        playerIsVisible = false;
        canChase = true;

        animator = GetComponentInChildren<Animator>();
        patrol = GetComponent<EnemyPatrol>();
        eyeControl = GetComponentsInChildren<EyeControl>();
        enemyAttack = GetComponentInChildren<EnemyMeleeAttack>();
        health = GetComponent<EnemyHealth>();
    }
    void Update()
    {   
        // If the enemy can see the player
        if (playerIsVisible)
        {
            // If the player is in range
            if (Vector2.Distance(transform.position, target.transform.position) < detectDistance)
            {
                exclamation.SetActive(true);    // Show the exclamation point on top of the enemy's head
            }
            if (canChase)
            {
                ChasePlayer();
            }            
        }
        // If the enemy can't see the player then he's on patrol
        else
        {
            exclamation.SetActive(false);
            enemySpeed = 2;
            patrol.Patrol();
        }
        // The eyes of the enemy follow the player if he's in sight
        EyesFollowPlayer(playerIsVisible);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player enters the collider directly in front of him, the enemy do his melee attack
        if(collision.gameObject.tag == "Player" && canAttack)
        {
            canAttack = false;
            canChase = false;
            playerHealth = collision.transform.GetComponent<PlayerHealth>();
            enemyAttack.PlayerHealth = playerHealth; // Set the reference of playerHealth for animation event
            animator.SetBool("Attack", true);
            StartCoroutine(AnimationEnd(0.41f));
        }
    }

    // If the enemy is no longer in range for a melee attack then the enemy has to chase him
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canChase = true;
        }
    }

    // Check if a fireball shooted by the player touched the enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FireBallPlayer") // If the fireball toutch any part of the enemy body then he dies
        {
            health.TakeDamage(1);
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player" && canAttack)
    //    {
    //        print("STAY");
    //        canAttack = false;
    //        canChase = false;
    //        playerHealth = collision.transform.GetComponent<PlayerHealth>();
    //        animator.SetBool("Attack", true);
    //        StartCoroutine(AnimationEnd(0.4f));
    //    }

    //}

    // We wait until the animation has ended before attacking again
    IEnumerator AnimationEnd(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);
        animator.SetBool("Attack", false);
        canAttack = true;
    }

    // Make the enemy chase after the player
    private void ChasePlayer()
    {
        // Direction the enemy must chase
        Vector3 direction = new Vector3(target.transform.position.x, 0, 0) - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        enemySpeed = 5;
        this.transform.position = Vector2.MoveTowards(transform.position, target.position, enemySpeed * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        // Check if the player is directly in front of the Melee enemy and if he's in his sight
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right * transform.localScale.x, detectDistance, ~layerMask);
        if (ray.collider != null)
        {
            // If the ray hit the player then he's visible to the enemy
            playerIsVisible = ray.collider.CompareTag("Player");
            if (playerIsVisible)
            {    
                Debug.DrawRay(transform.position, transform.right * detectDistance * transform.localScale.x, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.right * detectDistance * transform.localScale.x , Color.red);
            }
        }
        else
        {
            playerIsVisible = false;
        }
    }

    // Make the enemy eyes follow the player
    private void EyesFollowPlayer(bool isVisible)
    {
        foreach (EyeControl eye in eyeControl)
        {
            eye.enabled = isVisible;
        }
    }

}
