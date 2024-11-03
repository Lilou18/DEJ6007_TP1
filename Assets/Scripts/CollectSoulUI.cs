using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectSoulUI : MonoBehaviour
{
    // Update the UI when a soul is collected by the player

    private TMP_Text soulsText;
    private int numberSouls;

    private void Start()
    {
        numberSouls = 0;
        soulsText = GetComponent<TMP_Text>();
        CollectSoul.OnSoulCollected += UpdateSoulsText;
    }

    // Update the numbres of souls collected
    private void UpdateSoulsText()
    {
        numberSouls++;
        soulsText.text = numberSouls.ToString();
    }

    private void OnDisable()
    {
        CollectSoul.OnSoulCollected -= UpdateSoulsText;
    }
}
