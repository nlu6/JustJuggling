using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumBallsSlider : MonoBehaviour
{
    public Slider slider;
    public static int numBalls;
    // Start is called before the first frame update
    void Start()
    {
        numBalls = (int)slider.value;
    }
}
