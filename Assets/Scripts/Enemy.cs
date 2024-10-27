using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float detectDistance;
    Animator animator;

    PlayerHealth playerHealth;
    private bool canChase;
    private int enemyDamage;
    bool canAttack;

    [SerializeField] GameObject exclamation;
    

    [SerializeField] Transform target;
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float enemySpeed;
    private int nextPointPatrol;
    

    
    private Vector3 initScale;
    private bool playerIsVisible;

    bool showExclamationPoint;

    [SerializeField] EyeFollowPlayer[] eyes;

    void Start()
    {
        canAttack = true;
        enemyDamage = 1;
        animator = GetComponentInChildren<Animator>();
        nextPointPatrol = 0;
        initScale = transform.localScale;
        playerIsVisible = false;
        canChase = true;
    }
    void Update()
    {

        if (Vector2.Distance(transform.position, target.transform.position) > 4f)
        {
            showExclamationPoint = true;
        }
        if (playerIsVisible)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < 4f)
            {
                //StartCoroutine(ShowExclamation());
            }
            if (canChase)
            {
                ChasePlayer();
            }

        }
        else
        {
            enemySpeed = 2;
            Patrol();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && canAttack)
        {
            print("Enter");
            canAttack = false;
            canChase = false;
            playerHealth = collision.transform.GetComponent<PlayerHealth>();
            animator.SetBool("Attack", true);
            StartCoroutine(AnimationEnd(1f));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("EXIT");
            canChase = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && canAttack)
        {
            print("STAY");
            canAttack = false;
            canChase = false;
            playerHealth = collision.transform.GetComponent<PlayerHealth>();
            animator.SetBool("Attack", true);
            StartCoroutine(AnimationEnd(1f));
        }

    }
    IEnumerator AnimationEnd(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);
        TakeDamage();
        animator.SetBool("Attack", false);
        canAttack = true;
        print("canAttack");
    }

    private void TakeDamage()
    {
       // animator.SetBool("Attack", false);
        playerHealth.TakeDamage(enemyDamage);

    }
    private void ChasePlayer()
    {
        Vector3 direction = new Vector3(target.transform.position.x, 0, 0) - new Vector3(transform.position.x, transform.position.y, transform.position.z);
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
                // THIs IS A PROBLEM MULTIPLE COROUTINE AT THE SAME TIME!!
                
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
