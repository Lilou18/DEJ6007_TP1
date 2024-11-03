using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health;
    public int Health { get { return health; } }

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
