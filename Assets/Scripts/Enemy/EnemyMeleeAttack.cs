using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    // This class is necessary because we need a script to apply damage to the player 
    // on the same GameObject used for the animation event (animation event can't find script on parent GameObject)

    private int enemyDamage; // Damage applied to the player by the enemy

    EnemyMelee enemyMelee;
    PlayerHealth health;
    public PlayerHealth PlayerHealth { get { return health; } set { if (value != null) { health = value; } } } // Get a reference of the player health

    private void Start()
    {
        enemyDamage = 1;
        enemyMelee = GetComponentInParent<EnemyMelee>();
    }

    // Animation event that is called when the Melee enemy attack
    public void Attack()
    {
        SoundManager.Instance.PlaySound("MeleeMonsterAttack");
        if (enemyMelee.isPlayerInRange) // Check if the player is still in the monster range attack
        {
            if (PlayerHealth != null)
            {
                PlayerHealth.TakeDamage(enemyDamage);
            }
        }
        
    }
}
