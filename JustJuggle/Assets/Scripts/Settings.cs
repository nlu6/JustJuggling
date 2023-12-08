using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;


public class Settings : MonoBehaviour
{
    private int numObjects = 3;
    private float slowFactor = 3.5f;

    void Start() {
        // load settings from user prefs
        if(PlayerPrefs.GetFloat("SlowFactor") != 0)
        {
            slowFactor = PlayerPrefs.GetFloat("SlowFactor");
        }
        if(PlayerPrefs.GetInt("NumObjects") != 0)
        {
            numObjects = PlayerPrefs.GetInt("NumObjects");
        }

    }

    public void OnMenuButtonClick() {
        SceneManager.LoadScene("Start_Screen");
    }

    public void OnSaveButtonClick()
    {
        // save settings to user prefs
        PlayerPrefs.SetFloat("SlowFactor", slowFactor);
        PlayerPrefs.SetInt("NumObjects", numObjects);

        Debug.Log("Slow Factor: " + slowFactor);
        Debug.Log("Num Objects: " + numObjects);
    }
    
    public void SlowFactorSlider( float newValue ) {
        Debug.Log("Slow Factor: " + newValue);
        slowFactor = newValue;
    }

    public void NumberBallsSlider(int newValue) { 
        numObjects = newValue;
    }
    
}
