using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectSoulUI : MonoBehaviour
{
    private TMP_Text soulsText;
    private int numberSouls;

    private void Start()
    {
        numberSouls = 0;
        soulsText = GetComponent<TMP_Text>();
        CollectSoul.OnSoulCollected += UpdateSoulsText;
    }

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
