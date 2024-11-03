using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectSoul : MonoBehaviour, ICollectable
{
    // When a soul is collected we call the event to update the UI then we destroy it

    public static event Action OnSoulCollected;
    public void Collect()
    {
        OnSoulCollected.Invoke();
        Destroy(this.gameObject);
    }
}
