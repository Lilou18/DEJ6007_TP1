using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeControl : MonoBehaviour
{
    // Manage the movement of the eyes so they follow a target (player)

    [SerializeField] GameObject eye; 
    [SerializeField] Transform target;  // The player   

    public float intensity; // Radius around which the eye will move


    private void Update()
    {
        EyeAim();
    }

    void EyeAim()
    {
        Vector3 direction = target.position - this.transform.position;
        direction = Vector3.ClampMagnitude(direction, intensity);

        eye.transform.position = Vector3.Lerp(eye.transform.position, this.transform.position + direction, 25 * Time.deltaTime);
    }
}


