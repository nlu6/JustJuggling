using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Over : MonoBehaviour
{
    public void OnMenuButtonClick()
    {
        SceneManager.LoadScene("Start_Screen");
    }
}
