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
    [SerializeField] float enemySpeed;
    

    
    private Vector3 initScale;
    private bool playerIsVisible;

    EnemyPatrol patrol;
    EyeControl[] eyeControl;

    void Start()
    {
        canAttack = true;
        enemyDamage = 1;      
        initScale = transform.localScale;
        playerIsVisible = false;
        canChase = true;

        animator = GetComponentInChildren<Animator>();
        patrol = GetComponent<EnemyPatrol>();
        eyeControl = GetComponentsInChildren<EyeControl>();
    }
    void Update()
    {        
        if (playerIsVisible)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < 4f)
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
    private void EyesFollowPlayer(bool isVisible)
    {
        foreach (EyeControl eye in eyeControl)
        {
            eye.enabled = isVisible;
        }
    }

}
