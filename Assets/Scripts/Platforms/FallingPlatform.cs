using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] float fallWaitTime;
    [SerializeField] float destroyWaitTime;

    bool isFalling;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isFalling = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isFalling)
        {
            StartCoroutine(Falling());
            
        }
    }

    IEnumerator Falling()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallWaitTime);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(this.gameObject, destroyWaitTime);

    }
}
