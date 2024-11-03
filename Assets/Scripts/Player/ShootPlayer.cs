using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerFireBallPrefab;
    [SerializeField] float fireBallSpeed;

    private int maxFireBall; // Maximum number of fireball

    [SerializeField] private float attackCooldown; // Cooldown between each fireball
    private float lastShootTime;

    private void Start()
    {
        lastShootTime = 0f;
        attackCooldown = 1f;
        maxFireBall = 5;
    }

    private void Update()
    {
        // When the player left click with his mouse he shoots fireball
        if (Input.GetMouseButtonDown(0))
        {
            ShootFireBallPlayer();
        }
    }

    // Allow the player to shoot fireball in the mouse direction
    private void ShootFireBallPlayer()
    {
        // Cooldown between each attack
        if (Time.time >= lastShootTime + attackCooldown && maxFireBall > 0)
        {
            lastShootTime = Time.time;
            maxFireBall--;

            // We will fire in the mouse direction
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector3 shootDirection = (mousePosition - this.transform.position).normalized;

            GameObject fireBall = Instantiate(playerFireBallPrefab, transform.position, Quaternion.identity);

            //// Apply rotation to the sprite so it's facing the right direction
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            fireBall.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            fireBall.GetComponent<Rigidbody2D>().velocity = shootDirection * fireBallSpeed;

            SoundManager.Instance.PlaySound("Fireball");
        }
    }
}
