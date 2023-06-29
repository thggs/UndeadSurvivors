using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtons : MonoBehaviour
{
    [SerializeField] public Button[] allButtons; // Array of all buttons
    [SerializeField] private int numberOfButtonsToShow = 3; // Number of buttons to display

    private void Start(){
        //SelectRandomButtons();
        string bt = allButtons[3].name;
        Debug.Log(bt);
        
        allButtons[3].gameObject.SetActive(true);
    }

    private void SelectRandomButtons(){
        // Create a list to store the selected buttons
        //Button[] selectedButtons;

        int nButtons = allButtons.Length;
        int[] randomNumbers = GenerateRandomNumbers(numberOfButtonsToShow,1,nButtons);

        Debug.Log(randomNumbers[0]);
        

        // Select the first numberOfButtonsToShow buttons from the shuffled array
        for (int i = 0; i < (numberOfButtonsToShow-1); i++)
        {
            allButtons[randomNumbers[i]-1].gameObject.SetActive(true); // Show the selected button
        }

    }


    private int[] GenerateRandomNumbers(int count, int min, int max){
        int[] numbers = new int[count];

        for (int i = 0; i < count; i++)
        {
            numbers[i] = Random.Range(min, max + 1);
        }

        return numbers;
    }

}
