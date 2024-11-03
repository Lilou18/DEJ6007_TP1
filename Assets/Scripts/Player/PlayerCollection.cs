using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollection : MonoBehaviour
{
    // Manage the player's collection of soul item;
    public int NumberSouls { get; private set; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player touch a soul he collects them
        ICollectable collectible = collision.GetComponent<ICollectable>();
        if(collectible != null)
        {
            if(collision.gameObject.tag == "Soul")
            {
                AddSouls();
                SoundManager.Instance.PlaySound("Soul");
            }
            collectible.Collect();
        }
    }
    // Keep track of the number of souls the player collected
    public void AddSouls()
    {
        NumberSouls++;
    }
}
