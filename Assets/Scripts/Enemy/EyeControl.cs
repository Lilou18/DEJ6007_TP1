using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeControl : MonoBehaviour
{
    [SerializeField] GameObject eye;
    [SerializeField] Transform target;   

    public float intensity;


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


