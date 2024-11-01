using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushingBlock : MonoBehaviour
{
    [SerializeField] private float waitTimer;
    //[SerializeField] private float rise;
    //[SerializeField] private float fall;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float upSpeed;

    private Vector3 initialPosition;
    [SerializeField] private Transform fallingPosition;
    [SerializeField] private float fallingTimer;
    [SerializeField] private float fallingCooldown;

    private bool isFalling;
    private bool isWaiting;
    private void Start()
    {        
        initialPosition = transform.position;
        isFalling = true;
        fallingTimer = 0;
    }
    private void Update()
    {
        if (isFalling)
        {
            fallingTimer += Time.deltaTime;
            if (fallingTimer >= fallingCooldown)
            {
                Fall();
            }
        }
        else if(!isFalling && !isWaiting)
        {
            Rise();
        }
    }

    private void Fall()
    {
        // Descend le bloc vers la position de chute
        transform.position = Vector2.MoveTowards(transform.position, fallingPosition.position, fallSpeed * Time.deltaTime);

        // Vérifie si le bloc a atteint la position de chute
        if (Vector2.Distance(transform.position, fallingPosition.position) < 0.1f)
        {
            isFalling = false;  // Passe en mode montée
            fallingTimer = 0f;  // Remet le timer à zéro pour la prochaine séquence
            StartCoroutine(StartRising());
        }
    }

    private void Rise()
    {
        // Monte le bloc vers la position initiale
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, upSpeed * Time.deltaTime);

        // Vérifie si le bloc a atteint la position initiale
        if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
        {
            isFalling = true;  // Repasse en mode chute
            fallingTimer = 0f;  // Reset du timer pour la prochaine descente
        }
    }

    private IEnumerator StartRising()
    {
        isWaiting = true;
        // Attend avant de monter
        yield return new WaitForSeconds(waitTimer);
        isFalling = false;  // Commence la montée après le délai
        isWaiting = false;
    }

    //private void Update()
    //{
    //    fallingTimer += Time.deltaTime;
    //    print(this.transform.position.y);
    //    print(initialPosition.y);
    //    print(fallingTimer);
    //    if (this.transform.position.y >= initialPosition.y && fallingTimer > fallingCooldown)
    //    {

    //        Fall();
    //    }
    //    else if (this.transform.position.y <= fallingPosition.position.y)
    //    {
    //        StartCoroutine(RiseBlock());
    //    }

    //}

    //private void Fall()
    //{
    //    this.transform.position = Vector2.MoveTowards(transform.position, fallingPosition.position, fallSpeed * Time.deltaTime);
    //}

    //private IEnumerator RiseBlock()
    //{
    //    yield return new WaitForSeconds(waitTimer);
    //    transform.position = Vector2.MoveTowards(transform.position, initialPosition, upSpeed * Time.deltaTime);

    //}

}
