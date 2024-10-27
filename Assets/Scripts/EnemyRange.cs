using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;

public class EnemyRange : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float detectRange;
    private bool isPlayerVisible;

    EnemyPatrol enemyPatrol;
    EyeControl[] eyeControl;


    [SerializeField] GameObject fireBulletPrefab;
    [SerializeField] Transform firePoint; // Position where the fireball is instantiated
    [SerializeField] private float fireBallSpeed;
    private float attackCooldown;
    private float offsetFireBall; // We can't instantiate the fireball at the enemy position because of the collider so we add an offset.

    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyPatrol = GetComponent<EnemyPatrol>();
        eyeControl = GetComponentsInChildren<EyeControl>();
        detectRange = 10f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootFireBall();
        }
        
        if (isPlayerVisible)
        {
            EyesFollowPlayer(true);
        }
        else
        {
            EyesFollowPlayer(false);
            enemyPatrol.Patrol();
        }
    }

    private void FixedUpdate()
    {
        IsPlayerVisible();
    }

    // LES BOULES DE FEU SONT ENTRIGGER DONC TRAVRESES LES PLATEFORMES ET AUTRES!!!!!
    private void ShootFireBall()
    {
        Vector3 dirFireBall = (player.position - transform.position).normalized;
        Vector3 spawnPosition = transform.position + dirFireBall * offsetFireBall;

        GameObject fireBall = Instantiate(fireBulletPrefab, spawnPosition, Quaternion.identity);
        fireBall.GetComponent<Rigidbody2D>().velocity = new Vector2(dirFireBall.x, dirFireBall.y) * fireBallSpeed;
    }

    private void IsPlayerVisible()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - this.transform.position, detectRange);
        if(hit.collider != null)
        {
            isPlayerVisible = hit.collider.CompareTag("Player");
            if(isPlayerVisible)
            {
                Debug.DrawRay(transform.position, player.position - transform.position, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, player.position - transform.position, Color.red);
            }
        }
        else
        {
            isPlayerVisible = false;
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
