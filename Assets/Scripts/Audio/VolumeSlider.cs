using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    void Start () {
        PlayerPrefs.SetFloat("volume", 1f);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }

    public void SetLevel (float sliderValue)
    {
        PlayerPrefs.SetFloat("volume", sliderValue);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }
}
