using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSensitivity : MonoBehaviour
{
    public float horizontalSensitivity = 1f;
    public float verticalSensitivity = 1f;

    private CinemachineFreeLook _cinemachine;
    private float _baseYSensitivity;
    private float _baseXSensitivity;

    private void Start()
    {
        _cinemachine = GetComponent<CinemachineFreeLook>();
        _baseXSensitivity = _cinemachine.m_XAxis.m_MaxSpeed;
        _baseYSensitivity = _cinemachine.m_YAxis.m_MaxSpeed;
    }

    void Update() {
        if(PlayerPrefs.GetFloat("sens") != horizontalSensitivity) {
            horizontalSensitivity = PlayerPrefs.GetFloat("sens");
            _cinemachine.m_XAxis.m_MaxSpeed = _baseXSensitivity * horizontalSensitivity;
        }
    }

    public void OnChangeSensitivity(float x, float y)
    {
        _cinemachine.m_YAxis.m_MaxSpeed = _baseYSensitivity * verticalSensitivity;
        _cinemachine.m_XAxis.m_MaxSpeed = _baseXSensitivity * horizontalSensitivity;
    }
}
