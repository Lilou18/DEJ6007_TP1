using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    // This class manage the tutorial shown at the start of the game

    [SerializeField] private Sprite[] tutorialSprite;
    [SerializeField] private GameObject panel; // Tutorial panel
    [SerializeField] private GameObject image;
    [SerializeField] private Button button;
    private int indexSprites;

    private void Start()
    {
        //if (PlayerPrefs.GetInt("TutorialShown", 0) == 0)  // Did the player already see the tutorial?
        //{
        //    indexSprites = 0;
        //    ShowNextPicture();
        //    button.onClick.AddListener(ShowNextPicture); // Create onClick event
        //}
        //else
        //{
        //    panel.SetActive(false);
        //}
       
        
            indexSprites = 0;
            ShowNextPicture();
            button.onClick.AddListener(ShowNextPicture); // Create onClick event
        


    }

    // When the player click on the tutorial button it shows him the differents picture with the instructions
    public void ShowNextPicture()
    {
        // Show the next image in the tutorial
        if(indexSprites < tutorialSprite.Length)
        {
            image.GetComponent<Image>().sprite = tutorialSprite[indexSprites];
            
        }
        // The next time the player push the button it will close the tutorial panel
        if (indexSprites >= tutorialSprite.Length - 1)
        {
            button.GetComponentInChildren<TMP_Text>().text = "Close";
            button.onClick.RemoveListener(ShowNextPicture);
            button.onClick.AddListener(ClosePanel);
        }
        indexSprites++;
    }

    // Close the tutorial panel
    public void ClosePanel()
    {
        PlayerPrefs.SetInt("TutorialShown",1); // We dont want to see the tutorial each time we start the game
        panel.SetActive(false);
    }
}
