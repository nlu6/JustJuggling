using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class NumberDisplay : MonoBehaviour
{
    public TMP_Text BonusInput;
    public TMP_Text UI_Text;
    private int randomNumber;
    private bool displayingNumber = false;
    List<int> possibleNumbers = new List<int> { 6, 7, 8, 9, 0 };

    void Start()
    {
        // Hide the text initially
        BonusInput.text = "";
    }

    void Update()
    {
        if (!displayingNumber)
        {
            // Generate a random number and display it for 5 seconds
            StartCoroutine(DisplayRandomNumber());
        }

        // Check for keyboard input
        CheckKeyboardInput();
    }

    IEnumerator DisplayRandomNumber()
    {
        displayingNumber = true;

        // Generate a random number in 6, 7, 8, 9, 0
        randomNumber = possibleNumbers[Random.Range(0, possibleNumbers.Count)];
        BonusInput.text = randomNumber.ToString();

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Hide the text
        BonusInput.text = "";

        // Wait for a random time between 20 and 40 seconds
        float waitTime = Random.Range(20f, 41f);
        yield return new WaitForSeconds(waitTime);

        displayingNumber = false;
        // Display a new random number
    }

    void CheckKeyboardInput()
    {
        if (displayingNumber)
        {
            // Check if any key is pressed
            if (Input.anyKeyDown)
            {
                // Get the input and try to parse it as an integer
                string inputKey = Input.inputString;
                if (int.TryParse(inputKey, out int parsedInput))
                {
                    // Compare the input with the displayed number
                    if (parsedInput == randomNumber)
                    {
                        // Add 500 to the UI_Text score
                        int currentScore = int.Parse(UI_Text.text.Split(' ')[1]); // Extract current score
                        currentScore += 500; // Add 500
                        UI_Text.text = "Score: " + currentScore; // Update the score display
                        BonusInput.text = "";
                    }
                }
            }
        }
    }
}
