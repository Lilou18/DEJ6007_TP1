using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectSoul : MonoBehaviour, ICollectable
{
    public static event Action OnSoulCollected;
    public void Collect()
    {
        OnSoulCollected.Invoke();
        Destroy(this.gameObject);
    }
}
