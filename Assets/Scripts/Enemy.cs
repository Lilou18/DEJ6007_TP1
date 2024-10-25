using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] private float detectDistance;
    [SerializeField] private float colliderDistance;
    Animator animator;

    Rigidbody2D rbEnemy;
    private bool isChasingPlayer;

    [SerializeField] Transform target;
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float enemySpeed;
    private int nextPointPatrol;
    

    
    private Vector3 initScale;
    private bool playerIsVisible;

    [SerializeField] EyeFollowPlayer[] eyes;
    void Start()
    {
        animator = GetComponent<Animator>();
        nextPointPatrol = 0;
        initScale = transform.localScale;
        playerIsVisible = false;
        rbEnemy = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        if (playerIsVisible)
        {
            print("couocu");
           // this.transform.Translate(new Vector3(target.position.x,transform.position.y,transform.position.z));
        }
        else
        {
            print("Not visible!");
            //Patrol();
        }
        //FindTarget();
        //ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (playerIsVisible)
        {
            this.transform.Translate(target.transform.position);
            //rbEnemy.velocity = new Vector2
        }
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

    private bool IsPlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * detectDistance * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * detectDistance, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * detectDistance * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * detectDistance, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

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

}
