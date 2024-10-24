using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float enemySpeed;
    private int nextPointPatrol;
    private float detectDistance;

    Animator animator;
    private Vector3 initScale;
    

    [SerializeField] EyeFollowPlayer[] eyes;
    void Start()
    {
        animator = GetComponent<Animator>();
        detectDistance = 10f;
        nextPointPatrol = 0;
        initScale = transform.localScale;
    }

    
    void Update()
    {

        Patrol();
        //FindTarget();
    }

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
