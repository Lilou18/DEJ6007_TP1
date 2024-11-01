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
    [SerializeField] private float fallingTimer;  // Create a delay at the start so the crushing blocks dont fall at the same time
    [SerializeField] private float fallingCooldown;

    private bool canFall;
    private bool isWaiting;
    private void Start()
    {        
        initialPosition = transform.position;
        canFall = true;
        fallingTimer = 0;
    }
    private void Update()
    {
        if (canFall)
        {
            fallingTimer += Time.deltaTime;
            if (fallingTimer >= fallingCooldown)
            {
                Fall();
            }
        }
        else if(!canFall && !isWaiting)
        {
            Rise();
        }
    }

    private void Fall()
    {
        // Descend le bloc vers la position de chute
        transform.position = Vector2.MoveTowards(transform.position, fallingPosition.position, fallSpeed * Time.deltaTime);

        // V�rifie si le bloc a atteint la position de chute
        if (Vector2.Distance(transform.position, fallingPosition.position) < 0.1f)
        {
            canFall = false;  // Passe en mode mont�e
            fallingTimer = 0f;  // Remet le timer � z�ro pour la prochaine s�quence
            StartCoroutine(StartRising());
        }
    }

    private void Rise()
    {
        // Monte le bloc vers la position initiale
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, upSpeed * Time.deltaTime);

        // V�rifie si le bloc a atteint la position initiale
        if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
        {
            canFall = true;  // Repasse en mode chute
            fallingTimer = 0f;  // Reset du timer pour la prochaine descente
        }
    }

    private IEnumerator StartRising()
    {
        isWaiting = true;
        // Attend avant de monter
        yield return new WaitForSeconds(waitTimer);
        canFall = false;  // Commence la mont�e apr�s le d�lai
        isWaiting = false;
    }
}
