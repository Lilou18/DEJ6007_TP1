using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // This class manage the enemies health

    [SerializeField] private int health;
    public int Health { get { return health; } }

    // Reduce the health of the enemy and destroy it when he dies
    public void TakeDamage(int damage)
    {
        health -= damage;
        SoundManager.Instance.PlaySound("Hurt");
        if (Health <= 0)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }
}
