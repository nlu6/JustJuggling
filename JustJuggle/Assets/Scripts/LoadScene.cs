using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Load_Screen : MonoBehaviour
{
    public TMP_Text uitHighScore;
    public TMP_Text uitHighJuggles;

    void Start()
    {
        // get high score and high juggles from player prefs
        int highScore = PlayerPrefs.GetInt("HighScore");
        int highJuggles = PlayerPrefs.GetInt("HighJuggles");
        Debug.Log("High Score: " + highScore);

        // display high score and high juggles
        uitHighScore.text = "High Score: " + highScore;
        uitHighJuggles.text = "High Juggles: " + highJuggles;
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("Game_Screen");
    }

    public void OnSettingsButtonClick()
    {
        SceneManager.LoadScene("Settings_Screen");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }


}