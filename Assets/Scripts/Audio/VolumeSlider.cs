using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider mSlider;
    void Start () {
        mSlider.value = PlayerPrefs.GetFloat("volume");
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }
    public void SetLevel (float sliderValue)
    {
        PlayerPrefs.SetFloat("volume", sliderValue);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }
}
