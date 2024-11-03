using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;

public class EnemyRange : MonoBehaviour
{
    // This class manage the behaviour of the range enemy

    [SerializeField] Transform player; // The player the enemy wants to kill
    [SerializeField] float detectRange; // The range of his sight
    private bool isPlayerVisible;

    EnemyPatrol enemyPatrol;    // Allow the enemy to patrol between multiple points
    EyeControl[] eyeControl;    // Allow the eye of the enemy to follow the player


    [SerializeField] GameObject fireBulletPrefab;   // Projectile/Fireball the enemy is shooting
    [SerializeField] Transform pupil;   // Initiate the fireball at the pupil position
    [SerializeField] private float fireBallSpeed;
    [SerializeField] private float recoilDistance; // Offset to calculate the recoil position when shooting

    // The enemy has a cooldown between each shooting
    [SerializeField] private float attackCooldown; 
    private float lastShootTime;


    private Animator animator;

    [SerializeField] GameObject eye; // Eye of the enemy
    Color initEyeColor; // Initial color of the enemy eye

    [SerializeField] private LayerMask layerMask;

    EnemyHealth health;
    private void Awake()
    {
        initEyeColor = eye.gameObject.GetComponent<SpriteRenderer>().color;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyPatrol = GetComponent<EnemyPatrol>();
        eyeControl = GetComponentsInChildren<EyeControl>();
        health = GetComponent<EnemyHealth>();

        lastShootTime = 0f;
        attackCooldown = 1f;
    }

    private void Update()
    {        
        if (isPlayerVisible)
        {          
            EyesFollowPlayer(true);
            // We change the color of the eye to let the player know he's in the enemy sight
            eye.gameObject.GetComponent<SpriteRenderer>().color = new Color(192f / 255f, 32f / 255f, 30f / 255f, 1f);

            ShootFireBall();
        }
        else
        {
            // The eye color of the enemy becomes normal again when he doesn't see the player
            eye.gameObject.GetComponent<SpriteRenderer>().color = initEyeColor;

            EyesFollowPlayer(false);
            enemyPatrol.Patrol();
        }
    }

    private void FixedUpdate()
    {
        IsPlayerVisible();
    }

    // The enemy can shoot a Fireball, after a delay, to try and kill the player
    private void ShootFireBall()
    {
        // Check if the enemy can shoot
        if (Time.time >= lastShootTime + attackCooldown)
        {
            lastShootTime = Time.time;

            Vector3 dirFireBall = (player.position - transform.position).normalized;


            GameObject fireBall = Instantiate(fireBulletPrefab, pupil.transform.position, Quaternion.identity);

            // Apply rotation to the sprite so it's facing the right direction
            float angle = Mathf.Atan2(dirFireBall.y, dirFireBall.x) * Mathf.Rad2Deg;
            fireBall.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Move the bullet in the direction of the player
            fireBall.GetComponent<Rigidbody2D>().velocity = new Vector2(dirFireBall.x, dirFireBall.y) * fireBallSpeed;

            SoundManager.Instance.PlaySound("Fireball");

            Destroy(fireBall, 2f); // If it doesn't hit anything destroy it after 2 sec

            StartCoroutine(EyeRecoil(-dirFireBall));
        }
       
    }

    // Create a recoil effect when the enemy is shooting a fireball
    private IEnumerator EyeRecoil(Vector3 recoilDirection)
    { 
        Vector3 initPlayerPosition = transform.localPosition;

        Vector3 recoilPosition = initPlayerPosition + recoilDirection * recoilDistance;
        transform.localPosition = recoilPosition;

        yield return new WaitForSeconds(0.2f);
        transform.localPosition = initPlayerPosition; // He returns to his initial position
    }

    // Check if the player is in the line of sight of the enemy
    private void IsPlayerVisible()
    {
        // If the ray hit the player then he's visible to the enemy.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - this.transform.position, detectRange, ~layerMask);
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

    // Make the enemy eyes follow the player
    private void EyesFollowPlayer(bool isVisible)
    {
        foreach (EyeControl eye in eyeControl)
        {
            eye.enabled = isVisible;
        }
    }

    // Check if a fireball shooted by the player touched the enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FireBallPlayer") // If the fireball toutch any part of the enemy body then he dies
        {
            health.TakeDamage(1);
        }
    }


}
