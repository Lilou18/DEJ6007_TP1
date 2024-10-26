using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] private float detectDistance;
    [SerializeField] private float colliderDistance;
    Animator animator;

    PlayerHealth playerHealth;
    private bool canChase;

    Rigidbody2D rbEnemy;
    private bool isChasingPlayer;
    [SerializeField] GameObject exclamation;

    [SerializeField] Transform target;
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float enemySpeed;
    private int nextPointPatrol;
    

    
    private Vector3 initScale;
    private bool playerIsVisible;

    bool showExclamationPoint;

    [SerializeField] EyeFollowPlayer[] eyes;

    public float rangeAttack;
    private bool isInAttackRange;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        nextPointPatrol = 0;
        initScale = transform.localScale;
        playerIsVisible = false;
        rbEnemy = GetComponent<Rigidbody2D>();
        canChase = true;
    }

    bool test;
    void Update()
    {

        if (Vector2.Distance(transform.position, target.transform.position) > 4f)
        {
            showExclamationPoint = true;
        }
        if (playerIsVisible)
        {
            if(Vector2.Distance(transform.position,target.transform.position) < 4f)
            {
                StartCoroutine(ShowExclamation());
            }
            if (canChase)
            {
                ChasePlayer();
            }
            
        }
        else
        {
            enemySpeed = 2;
            //Patrol();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            canChase = false;
            playerHealth = collision.transform.GetComponent<PlayerHealth>();
            animator.SetBool("Attack", true);
            TakeDamage();
            StartCoroutine(AnimationEnd());
            isInAttackRange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    IEnumerator AnimationEnd()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("Attack", false);
    }

    private void TakeDamage()
    {
       // animator.SetBool("Attack", false);
        playerHealth.TakeDamage();

    }

    //private void DamagePlayer()
    //{
    //    if (IsPlayerInSight())
    //    {
    //        // Damage player
    //        playerInfo.TakeDamage();
    //    }
    //}

    private void ChasePlayer()
    {
        Vector3 direction = new Vector3(target.transform.position.x, 0, 0) - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //this.transform.Translate(new Vector3(target.position.x,transform.position.y,transform.position.z));
        this.transform.Translate(direction.normalized * Time.deltaTime * enemySpeed);
        enemySpeed = 3;
    }

    private void FixedUpdate()
    {
        //RaycastHit2D ray = Physics2D.Raycast(transform.position, target.transform.position - transform.position, 4f);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right * transform.localScale.x, 4f);
        if (ray.collider != null)
        {
            
            playerIsVisible = ray.collider.CompareTag("Player");
            if (playerIsVisible)
            {
                //StartCoroutine(ShowExclamation());
                
                Debug.DrawRay(transform.position, transform.right * 4f * transform.localScale.x, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.right * 4f * transform.localScale.x , Color.red);
            }
        }
        else
        {
            playerIsVisible = false;
        }
    }

    //private bool IsPlayerInSight()
    //{
    //    RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * detectDistance * transform.localScale.x * colliderDistance,
    //        new Vector3(boxCollider.bounds.size.x * detectDistance, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
    //        0, Vector2.left, 0, playerLayer);

    //    if(hit.collider != null)
    //    {
    //        playerInfo = hit.transform.GetComponent<PlayerInfo>();
    //    }

    //    return hit.collider != null;
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * detectDistance * transform.localScale.x * colliderDistance,
    //        new Vector3(boxCollider.bounds.size.x * detectDistance, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    //}

    // Make the enemy patrol between points
    private void Patrol()
    {
        if (Vector2.Distance(transform.position, patrolPoints[nextPointPatrol].position) <= 0.2f)
        {
            nextPointPatrol++;
            if (nextPointPatrol >= patrolPoints.Length)
            {
                nextPointPatrol = 0;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[nextPointPatrol].position, enemySpeed * Time.deltaTime);

            // Make sure the sprite is facing the good direction
            if (transform.position.x < patrolPoints[nextPointPatrol].position.x)
            {
                transform.localScale = new Vector3(initScale.x, initScale.y, initScale.z);
            }
            else
            {
                transform.localScale = new Vector3(initScale.x * -1, initScale.y, initScale.z);
            }
        }
    }
    private void FindTarget()
    {
        if (Vector2.Distance(transform.position, target.position) <= detectDistance)
        {
            foreach (EyeFollowPlayer eye in eyes)
            {
                eye.enabled = true;
            }
        }
        else
        {
            foreach (EyeFollowPlayer eye in eyes)
            {
                eye.enabled = false;
            }
        }
    }

    // Used to show the player the enemy saw him
    IEnumerator ShowExclamation()
    {
        if (showExclamationPoint && playerIsVisible)
        {
            showExclamationPoint = false;
            exclamation.SetActive(true);
            yield return new WaitForSeconds(1);
            exclamation.SetActive(false);
        }
        
    }

}
