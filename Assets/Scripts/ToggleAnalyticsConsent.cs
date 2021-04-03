using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAnalyticsConsent : MonoBehaviour
{
    public GameObject objectToShow;
    public GameObject objectToHide;
    public bool setAnalyticsOn;

    public void OnButtonPress()
    {
        Globals.analyticsOn = setAnalyticsOn;
        objectToShow.SetActive(true);
        objectToHide.SetActive(false);
    }
}
