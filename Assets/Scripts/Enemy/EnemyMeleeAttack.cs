using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{

    private int enemyDamage;
    PlayerHealth health;
    public PlayerHealth Health { get { return health; } set { if (value != null) { health = value; } } }

    private void Start()
    {
        enemyDamage = 1;
    }
    public void Attack()
    {
        if(Health != null)
        {
            Health.TakeDamage(enemyDamage);
        }
    }
}
