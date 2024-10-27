using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float enemySpeed;
    int nextPointPatrol;
    private Vector3 initScale;

    private void Start()
    {
        nextPointPatrol = 0;
        initScale = transform.localScale;
    }
    public void Patrol()
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
}
