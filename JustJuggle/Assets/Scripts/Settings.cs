using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public AudioMixer masterMixer;

    public void OnMenuButtonClick()
    {
        SceneManager.LoadScene("Start_Screen");
    }

    public void VolumeSlider(float volumeLevel) {

        masterMixer.SetFloat("Volume", volumeLevel);
    }

    public void InputDelaySlider()
    {

    }
}
