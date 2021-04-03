using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensSlider : MonoBehaviour
{
    void Start () {
        PlayerPrefs.SetFloat("sens", 1f);
    }

    public void SetSens (float sliderValue)
    {
        PlayerPrefs.SetFloat("sens", sliderValue);
    }
}
