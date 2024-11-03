using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    Vector3 lastCheckpoint; // Last checkpoint of the player
    SpriteRenderer playerSpriteRenderer;
    [SerializeField] Sprite spritePlayerHurt;

    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    private void Start()
    {
        lastCheckpoint = transform.position; // First checkpoint is at the start of the game
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        PlayerHealth.OnPlayerHurt += PlayerHurt;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player trigger a collision with a checkpoint then we update the respawn position
        if (collision.gameObject.tag == "Checkpoint")
        {
            SoundManager.Instance.PlaySound("Checkpoint");
            lastCheckpoint = collision.transform.position; // We update our checkpoint
            collision.gameObject.GetComponent<Collider2D>().enabled = false; // Keep the player from reactivating an old checkpoint
            collision.gameObject.GetComponentInChildren<Light2D>().enabled = true;
        }
    }

    // Make the player respawn
    private void PlayerHurt()
    {
        StartCoroutine(Respawn());
    }

    // When the player get's hurt we make him respawn to the last checkpoint or his initial position
    private IEnumerator Respawn()
    {
        // Keep him from moving
        playerMovement.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;

        // Change the color of the sprite to show that the player has been hurt
        Sprite initSprite = playerSpriteRenderer.sprite;
        playerSpriteRenderer.sprite = spritePlayerHurt;
        yield return new WaitForSeconds(0.5f);
        playerSpriteRenderer.sprite = initSprite;

        // Change the position of the player at the last checkpoint or the start of the game
        transform.position = lastCheckpoint;

        // Make sure the camera following script is activated (it's desactivated when the player falls into a hole)
        Camera.main.gameObject.GetComponent<CameraMovement>().enabled = true;

        // Allow him from moving again
        playerMovement.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerHurt -= PlayerHurt;
    }
}
