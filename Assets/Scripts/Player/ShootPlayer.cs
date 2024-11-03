using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShootPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerFireBallPrefab;
    [SerializeField] private float fireBallSpeed;

    private int numberFireBall; // Number of fireball the player can shoot

    [SerializeField] private float attackCooldown; // Cooldown between each fireball
    private float lastShootTime;

    private void Start()
    {
        lastShootTime = 0f;
        attackCooldown = 1f;
        numberFireBall = 0;
    }

    private void Update()
    {
        // When the player left click with his mouse he shoots fireball
        if (Input.GetMouseButtonDown(0))
        {
            ShootFireBallPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Flower")
        {
            numberFireBall = 5;
            StartCoroutine(WaitFlowerGrow(collision.gameObject));
        }
    }

    private IEnumerator WaitFlowerGrow(GameObject flower)
    {
        flower.GetComponentInChildren<Light2D>().enabled = false;
        flower.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(10f);
        flower.GetComponent<Collider2D>().enabled = true;
        flower.GetComponentInChildren<Light2D>().enabled = true;

    }

    // Allow the player to shoot fireball in the mouse direction
    private void ShootFireBallPlayer()
    {
        // Cooldown between each attack
        if (Time.time >= lastShootTime + attackCooldown && numberFireBall > 0)
        {
            lastShootTime = Time.time;
            numberFireBall--;

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
