using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDefaultPlayerPrefs : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetFloat("sens") == 0f && PlayerPrefs.GetFloat("volume") == 0f) {
            PlayerPrefs.SetFloat("sens", 0.3f);
            PlayerPrefs.SetFloat("volume", 0.3f);
        }
    }

    void Update()
    {
        
    }
}
