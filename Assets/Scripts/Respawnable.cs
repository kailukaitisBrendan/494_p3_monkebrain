using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawnable : MonoBehaviour
{
    private Vector3 initPosition;
    private Quaternion initRotation;

    // Start is called before the first frame update
    private void Start()
    {
        initPosition = transform.position;
        initRotation = transform.rotation;
    }

    public void Respawn()
    {
        transform.position = initPosition;
        transform.rotation = initRotation;
    }

}
