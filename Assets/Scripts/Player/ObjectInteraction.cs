using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ObjectInteraction : MonoBehaviour
{
    public float pickupDistance;
    public Transform itemSlot;
    public GameObject dolly;

    public float throwForce;
    public float throwAngle = 45f;
    
    private bool _hasItem;
    private bool _hasDolly;
    private GameObject _pickedUpObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_hasDolly)
            {
                DropDolly();
            }
            else
            {
                PickupDolly();
            }

        } else if (Input.GetKeyDown(KeyCode.E))
        {
            if (!_hasDolly) return;
            if (_hasItem)
            {
                DropItem();
            }
            else
            {
                PickupItem();
            }
        } else if (Input.GetMouseButtonDown(1))
        {
            if (!_hasItem || !_hasDolly) return; 
            ThrowItem();
        }
    }

    private void ThrowItem()
    {
        Debug.Log("Throw!");
        GameObject item = _pickedUpObject;
        // First, drop the object
        DropItem();
        // We want to throw object in direction the player is facing
        // This should be the transform.forward of our player object.
        // We need to apply an impulse force in the respective direction
        //Vector3 forceDirection = transform.forward.normalized * throwForce;

        float angle = throwAngle * Mathf.Deg2Rad;
        Vector3 angleDirection = new Vector3(0, Mathf.Sin(angle), 0f);
        Vector3 forceDirection = transform.forward + angleDirection;
        Debug.Log(forceDirection);
        item.GetComponent<Rigidbody>().AddForce(forceDirection * throwForce, ForceMode.Impulse);
        
        // Add Component to the package to alert enemies on colliding with ground.
        item.AddComponent<OnCollisionEvent>();

    }

    private void DropDolly()
    {
        // Dont drop dolly if we have an item in it!
        if (_hasItem) return;

        // Re-enable collider.
        dolly.GetComponent<Collider>().enabled = true;
        
        dolly.transform.parent = null;
        dolly = null;
        _hasDolly = false;

        // TODO: Enable jump
    }

    private void PickupDolly()
    {
        GameObject item = GetItem();
        if (item == null) return;
        if (!item.CompareTag("Dolly")) return;
        
        // Set parent to be player
        item.transform.SetParent(gameObject.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
        dolly = item;
        _hasDolly = true;
        
        // Disable collider so that it ignores all collision when
        // we pick it up.
        Collider col = item.GetComponent<Collider>();
        col.enabled = false;


        // TODO: Disable jump

    }

    private void DropItem()
    {
        // Re-enable rigidbody and collider.
        Rigidbody rb = _pickedUpObject.GetComponent<Rigidbody>();
        
        Collider col = _pickedUpObject.GetComponent<Collider>();
        
        // Set the parent back to null
        _pickedUpObject.transform.parent = null;
        rb.isKinematic = false;
        col.enabled = true;

        _pickedUpObject = null;
        _hasItem = false;
        
        

        //Debug.Log("Dropped Item");
    }

    private void PickupItem()
    {
        
        GameObject item = GetItem();
        if (item == null) return;
        if (!item.CompareTag("Package")) return;
        // Get the rigidbody of our hit.
        Rigidbody rb = item.GetComponent<Rigidbody>();
        //Disable the rigidbody and rest velocities 
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        // Set parent as the player.
        item.transform.SetParent(itemSlot);
        // Reset the local position and rotation.
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
        
        // Disable collider of grabbed object.
        //Collider col = item.GetComponent<Collider>();
        //col.enabled = false;
        // Ignore collisions between player and package

        _hasItem = true;
        _pickedUpObject = item;
    }

    private GameObject GetItem()
    {
        // Raycast out from player to see if item is in front of the player.
        // Create mask so we only collide with pickup-able objects.
        LayerMask mask = LayerMask.GetMask("Grabbable Object");
        Vector3 pos = transform.position;
        pos.y = itemSlot.position.y;
        RaycastHit hit;
        if (Physics.Raycast(pos, transform.TransformDirection(Vector3.forward), out hit, pickupDistance, mask))
        {
            Debug.DrawRay(pos, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }

        return hit.transform == null ? null : hit.transform.gameObject;
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
