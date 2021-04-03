using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBlend : MonoBehaviour
{
    public GameObject freeLookCam;
    public GameObject aimCam;

    public void OnToggleThrowing()
    {
        freeLookCam.SetActive(!freeLookCam.activeSelf);
        aimCam.SetActive(!aimCam.activeSelf);
    }
}
