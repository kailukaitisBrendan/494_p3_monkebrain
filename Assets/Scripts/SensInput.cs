using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SensInput : MonoBehaviour
{
    void Update()
    {
        gameObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed *= PlayerPrefs.GetFloat("sens");
    }
}
