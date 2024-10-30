using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Vector3 lastCheckpoint; // Last checkpoint of the player
    SpriteRenderer playerSpriteRenderer;
    [SerializeField] Sprite spritePlayerHurt;

    private void Start()
    {
        lastCheckpoint = transform.position;
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        PlayerHealth.OnPlayerHurt += PlayerHurt;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            lastCheckpoint = collision.transform.position; // We update our checkpoint
            collision.gameObject.GetComponent<Collider2D>().enabled = false; // Keep the player from reactivating an old checkpoint
        }
    }

    private void PlayerHurt()
    {
        StartCoroutine(Respawn());
    }
    private IEnumerator Respawn()
    {
        Sprite initSprite = playerSpriteRenderer.sprite;
        playerSpriteRenderer.sprite = spritePlayerHurt;
        yield return new WaitForSeconds(0.5f);
        playerSpriteRenderer.sprite = initSprite;
        transform.position = lastCheckpoint;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerHurt -= PlayerHurt;
    }
}
