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
    }

    
    void Update()
    {
        Patrol();
        //FindTarget();
    }

    private void FixedUpdate()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward);
        if (hit.collider != null)
        {
            playerIsVisible = hit.collider.CompareTag("Player");
            if (playerIsVisible)
            {
                Debug.DrawRay(transform.position, Vector3.forward, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, Vector3.forward, Color.red);
            }
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
