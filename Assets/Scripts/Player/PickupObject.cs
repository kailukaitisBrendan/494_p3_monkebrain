using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public float pickupDistance;
    public Transform itemSlot;
    private bool _pickedUp;
    private GameObject _pickedUpObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Drop item if we are already holding something. 
            if (_pickedUp)
            {
                DropItem();
            }
            else
            {
                PickupItem();
            }
        }
    }

    private void DropItem()
    {
        // Re-enable rigidbody and collider.
        Rigidbody rb = _pickedUpObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Collider col = _pickedUpObject.GetComponent<Collider>();
        
        // Set the parent back to null
        _pickedUpObject.transform.parent = null;
        col.enabled = true;

        _pickedUp = false;
        
        //Re-enable collision.
        Physics.IgnoreCollision(col, GetComponent<Collider>(), false);
        
        Debug.Log("Dropped Item");
    }

    private void PickupItem()
    {
        // Try to pick up item in front of the player.
        // Create mask so we only collide with grabbable objects.
        LayerMask mask = LayerMask.GetMask("Interactable Object");
        // Shoot a ray out a short distance in front of player. 
        Vector3 pos = transform.position;
        pos.y = itemSlot.position.y;
        RaycastHit hit;
        if (Physics.Raycast(pos, transform.TransformDirection(Vector3.forward), out hit, pickupDistance, mask))
        {
            Debug.DrawRay(pos, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Item picked up");
        }
        
        _pickedUpObject = hit.transform.gameObject;
        
        // Get the rigidbody of our hit.
        Rigidbody rb = _pickedUpObject.GetComponent<Rigidbody>();
        //Disable the rigidbody and rest velocities 
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        // Set it as the player.
        _pickedUpObject.transform.SetParent(itemSlot);
        // Reset the local position and rotation.
        _pickedUpObject.transform.localPosition = Vector3.zero;
        _pickedUpObject.transform.localEulerAngles = Vector3.zero;
        
        // Ignore collision between the collider and the player.
        Collider col = _pickedUpObject.GetComponent<Collider>();
        Physics.IgnoreCollision(col, GetComponent<Collider>(), true);

        _pickedUp = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 pos = transform.position;
        pos.y = itemSlot.transform.position.y;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * pickupDistance;
        Gizmos.DrawRay(pos, direction);
    }
}
