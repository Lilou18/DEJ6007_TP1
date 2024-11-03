using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    // This class manage the behaviour of a platforms that fall when the player jump on it.

    [SerializeField] float shakeTime;   // The amount of time the platform is shaking before falling
    [SerializeField] float destroyWaitTime; // Time to wait beofre the platofrm get destroyed
    [SerializeField] private Vector3 shakeAmount; // How much we want the platform to shake

    bool isFalling;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isFalling = false;
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
        Destroy(this.gameObject, destroyWaitTime);
    }
}
