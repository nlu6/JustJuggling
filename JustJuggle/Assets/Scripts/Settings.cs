using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public AudioMixer masterMixer;
    public JugglingObject jugglingObject;
    public StartUp onStart;

    public void OnMenuButtonClick() {

        SceneManager.LoadScene("Start_Screen");
    }

    
    public void SlowFactorSlider( float newValue ) {
        jugglingObject.slowFactor = newValue;
    }

    public void NumberBallsSlider(int newValue) { 
        onStart.numObjects = newValue;
    }
    
}
