using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnabled : MonoBehaviour
{
    public UnityEvent onEnabled;

    private void OnEnable()
    {
        onEnabled.Invoke();
    }
}
