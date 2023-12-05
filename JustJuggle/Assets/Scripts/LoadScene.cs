using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_Screen : MonoBehaviour
{
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