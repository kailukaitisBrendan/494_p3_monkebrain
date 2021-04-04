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

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb) {
            rb.velocity = new Vector3(0f,0f,0f); 
            rb.angularVelocity = new Vector3(0f,0f,0f);
        }
    }

}
