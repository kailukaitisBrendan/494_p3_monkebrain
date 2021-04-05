using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensSlider : MonoBehaviour
{
    public Slider mSlider;
    void Start () {
        mSlider.value = PlayerPrefs.GetFloat("sens");
    }

    public void SetSens (float sliderValue)
    {
        PlayerPrefs.SetFloat("sens", sliderValue);
    }
}
