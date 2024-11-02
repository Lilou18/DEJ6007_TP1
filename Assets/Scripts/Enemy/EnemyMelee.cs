using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private float detectDistance;
    [SerializeField] Animator animator;

    PlayerHealth playerHealth;
    private bool canChase;
    bool canAttack;

    [SerializeField] GameObject exclamation;    

    [SerializeField] Transform target;
    [SerializeField] float enemySpeed;
    

    
    private Vector3 initScale;
    private bool playerIsVisible;

    EnemyPatrol patrol;
    EyeControl[] eyeControl;

    EnemyMeleeAttack enemyAttack;

    [SerializeField] private LayerMask layerMask;

    void Start()
    {
        canAttack = true;   
        initScale = transform.localScale;
        playerIsVisible = false;
        canChase = true;

        animator = GetComponentInChildren<Animator>();
        patrol = GetComponent<EnemyPatrol>();
        eyeControl = GetComponentsInChildren<EyeControl>();
        enemyAttack = GetComponentInChildren<EnemyMeleeAttack>();
    }
    void Update()
    {        
        if (playerIsVisible)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < detectDistance)
            {
                exclamation.SetActive(true);
            }
            if (canChase)
            {
                ChasePlayer();
            }            
        }
        else
        {
            exclamation.SetActive(false);
            enemySpeed = 2;
            patrol.Patrol();
        }
        EyesFollowPlayer(playerIsVisible);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        Vector3 direction = new Vector3(target.transform.position.x, 0, 0) - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        enemySpeed = 5;
        //this.transform.Translate(direction.normalized * Time.deltaTime * enemySpeed);
        this.transform.position = Vector2.MoveTowards(transform.position, target.position, enemySpeed * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        // Check if the player is directly in front of the Melee enemy and if he's in his sight
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right * transform.localScale.x, detectDistance, ~layerMask);
        if (ray.collider != null)
        {
            
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
