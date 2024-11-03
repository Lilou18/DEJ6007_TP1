using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollection : MonoBehaviour
{
    public int NumberSouls { get; private set; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
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

    public void AddSouls()
    {
        NumberSouls++;
    }
}
