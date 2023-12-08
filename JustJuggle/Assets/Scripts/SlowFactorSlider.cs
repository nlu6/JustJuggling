using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowFactorSlider : MonoBehaviour
{
    public Slider slider;
    public static float sFactor;
    // Start is called before the first frame update
    void Start()
    {
        sFactor = (float)slider.value;
    }
}
