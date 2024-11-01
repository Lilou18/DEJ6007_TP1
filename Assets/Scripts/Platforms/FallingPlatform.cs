using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] float shakeTime;
    [SerializeField] float destroyWaitTime;
    [SerializeField] private Vector3 shakeAmount;

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
            //StartCoroutine(Falling());
            isFalling = true;
            iTween.ShakePosition(this.gameObject, iTween.Hash("amount", shakeAmount, "time", shakeTime, "oncomplete", "Fall"));
            
        }
    }
    private void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(this.gameObject, destroyWaitTime);
    }


    //IEnumerator Falling()
    //{
    //    isFalling = true;
    //    yield return new WaitForSeconds(fallWaitTime);
    //    rb.bodyType = RigidbodyType2D.Dynamic;
    //    Destroy(this.gameObject, destroyWaitTime);

    //}
}
