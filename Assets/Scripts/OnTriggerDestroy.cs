using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Respawnable>()) {
            other.gameObject.GetComponent<Respawnable>().Respawn();
        }
        else {
            // Destroy(other.gameObject);
        }
    }
}
