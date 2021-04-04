using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBlend : MonoBehaviour
{
    public GameObject freeLookCam;
    public GameObject aimCam;

    private Subscription<ThrowingEvent> _throwingSubscription;

    private void Start()
    {
        _throwingSubscription = EventBus.Subscribe<ThrowingEvent>(OnToggleThrowing);
    }

    private void OnToggleThrowing(ThrowingEvent e)
    {
        freeLookCam.SetActive(!freeLookCam.activeSelf);
        aimCam.SetActive(!aimCam.activeSelf);
    }
}
