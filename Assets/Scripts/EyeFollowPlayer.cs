using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollowPlayer : MonoBehaviour
{

    [SerializeField] Transform targetPlayer;
    void Update()
    {
        Vector3 targetDir = targetPlayer.position - transform.position;
        float angle = (Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
