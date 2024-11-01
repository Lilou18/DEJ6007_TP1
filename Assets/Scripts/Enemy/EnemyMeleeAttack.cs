using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{

    private int enemyDamage;
    PlayerHealth health;
    public PlayerHealth PlayerHealth { get { return health; } set { if (value != null) { health = value; } } } // Get a reference to player health

    private void Start()
    {
        enemyDamage = 1;
    }

    // Animation event that is called when the Melee enemy attack
    public void Attack()
    {
        if(PlayerHealth != null)
        {
            PlayerHealth.TakeDamage(enemyDamage);
        }
    }
}
