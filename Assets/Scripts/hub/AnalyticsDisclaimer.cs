using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsDisclaimer : MonoBehaviour
{
    public GameObject mainaMenu;
    void Start() {
        if (PlayerPrefs.GetInt("PlayedIntro") == 1) {
            mainaMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
