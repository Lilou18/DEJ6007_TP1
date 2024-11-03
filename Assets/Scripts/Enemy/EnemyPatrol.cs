using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    // This class manage the patrol movement of an enemy between different points

    [SerializeField] Transform[] patrolPoints; // Points the enemy will patrol
    [SerializeField] float enemySpeed;
    int nextPointPatrol;
    private Vector3 initScale; // Used to change the direction the enemy is facing

    private void Start()
    {
        nextPointPatrol = 0;
        initScale = transform.localScale;
    }
    public void Patrol()
    {
        // If the enemy reaches his destination, he is given a new destination to patrol
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
            // Walks toward the destination
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
