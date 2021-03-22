using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithGround : MonoBehaviour
{

    public void Align(Vector3 normal) {
        transform.rotation = Quaternion.LookRotation(normal);
    }
}
