using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAudioOnAwake : MonoBehaviour
{
    private void Awake()
    {
        GameObject audio = GameObject.Find("SoundController");
        audio.SetActive(false);
    }
}
