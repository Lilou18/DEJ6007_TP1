using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    // This class manage the behaviour of a platforms that fall when the player jump on it.

    [SerializeField] float shakeTime;   // The amount of time the platform is shaking before falling
    [SerializeField] float resetWaitTime; // Time to wait before the platofrm reappears
    [SerializeField] private Vector3 shakeAmount; // How much we want the platform to shake

    bool isFalling;
    Rigidbody2D rb;

    private Vector3 initialPosition;    // Initial position of the platform

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isFalling = false;
        initialPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player jumps on the platform and it's not already falling
        if (collision.gameObject.tag == "Player" && !isFalling)
        {
            isFalling = true;
            // After the shake animation we call the function that make the platform fall
            iTween.ShakePosition(this.gameObject, iTween.Hash("amount", shakeAmount, "time", shakeTime, "oncomplete", "Fall"));
            
        }
    }

    // Make the platform fall then destroy it after a delay
    private void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // Change from Kinematic to Dynamic
        Invoke("ResetPlatform", resetWaitTime);
    }

    // Reset the platform position after falling in case the player missed the jump
    private void ResetPlatform()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
        transform.position = initialPosition;
        isFalling = false;
    }
}
