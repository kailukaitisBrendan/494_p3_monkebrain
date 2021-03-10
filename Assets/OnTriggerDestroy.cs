using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hello1");

        if (other.gameObject.CompareTag("Package"))
        {
            Debug.Log("Hello");
            Destroy(other.gameObject);
        }
    }
}
