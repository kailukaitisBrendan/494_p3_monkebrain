﻿using System;
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

    public LineRenderer lineRenderer;

    public float throwForce;
    public float maxForceMultiplier;
    public float throwAngle = 45f;

    public float charge_force_scaler = 1.5f;
    
    private float action_delay = 0.2f;
    private float action_t = 0.0f;

    private bool _hasItem;
    private bool _hasDolly;
    private GameObject _pickedUpObject;
    private float _currentForceMultiplier = 0.0f;
    private Rigidbody _pickedUpObjectRigidbody;

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || 
            (Input.GetMouseButton(0) && !_hasItem)) &&
            Time.time - action_t > action_delay)
        {
            action_t = Time.time;
            // If we do not have dolly, pick it up first
            if (!_hasDolly)
            {
                PickupDolly();
            }
            else if (!_hasItem && _hasDolly)
            {
                // if no item, pick up item.
                PickupItem();
                if (!_hasItem) {
                    DropDolly();
                }
            }
            // If we have an item, drop it first.
            else if (_hasItem)
            {
                DropItem();
            }
            else if (_hasDolly && !_hasItem)
            {
                Debug.Log("you should never see this");
                // No, item so drop dolly.
                DropDolly();
            }
        }
    }

    void FixedUpdate() {
        if (Input.GetMouseButton(0))
        {
            // We are holding down the mouse button, so charge up the force.
            if (!_hasItem || !_hasDolly) return;
            ChargeThrow();
        }
        if (!Input.GetMouseButton(0) && _currentForceMultiplier > 0.0f)
        {
            if (!_hasItem || !_hasDolly) return;
            // Throw item.
            ThrowItem();
            _currentForceMultiplier = 0.0f;
        }
    }

    private void ChargeThrow()
    {
        _currentForceMultiplier += Time.deltaTime * charge_force_scaler;
        
        // If we go over our maximum charge, then set it to max
        if (_currentForceMultiplier >= maxForceMultiplier)
        {
            _currentForceMultiplier = maxForceMultiplier;
        }
        
        DrawTrajectoryPath();
    }

    private void DrawTrajectoryPath()
    {
        List<Vector3> path = new List<Vector3>();
        lineRenderer.enabled = true;
        // ---- Draw trajectory path -----
        // To draw the trajectory path we need to simulate the projectile position across set intervals.
        // First, we need to calculate the total time the projectile will take before landing
        // This is given by the equation t = (vsin(θ) + sqr((vsin(θ)^2 + 2gy_0)) / g where
        // θ = Angle of the projectile
        // y_0 = the initial height of the projectile
        // v = the magnitude of the velocity vector
        // g = gravity

        Vector3 position = _pickedUpObject.transform.position;
        Vector3 velocity = CalculateVelocity(true);
        //Debug.Log(velocity);
        float v = velocity.magnitude;
        // Calculate magnitude
        // Since Physics.gravity.y returns a negative value, we have to convert to absolute value. 
        float totalTime = (v * Mathf.Sin(throwAngle) +
                            Mathf.Sqrt(Mathf.Pow(v * Mathf.Sin(throwAngle), 2) + Mathf.Abs(2 * Physics.gravity.y * position.y)));
        totalTime /= Mathf.Abs(Physics.gravity.y);
        //Debug.Log(totalTime);
        
        // Next, we need to simulate the flight path by calculating the position and 
        // velocity vectors over set time intervals.
        // For each interval, we add the position to the path list so we can draw the trajectory.
        float timeStep = Time.fixedDeltaTime;
        for (float t = 0f; t < totalTime; t += timeStep)
        {
            velocity += Physics.gravity * timeStep;
            position += velocity * timeStep;
            path.Add(position);
        }
        
        // Now, draw the trajectory using a LineRenderer.
        lineRenderer.positionCount = path.Count;
        lineRenderer.SetPositions(path.ToArray());
    }

    private Vector3 CalculateVelocity(bool mass=false)
    {
        // Since our charge throw force acts as a force multiplier, then we can calculate our force
        // from multiplying our values together with the angle. Thus, our velocity will be defined as
        // v = angleDirection + current forward direction * throw force * throw multiplier. 
        float angle = throwAngle * Mathf.Deg2Rad;
        Vector3 angleDir = new Vector3(0, Mathf.Sin(angle), 0);
        Vector3 force = (transform.forward + angleDir).normalized * (throwForce * _currentForceMultiplier);
        // If mass is true, then calculate the velocity using the mass of the object.
        // The velocity is given by the equation v = p / m, where
        // p = the momentum of the object
        // m = the mass of the object.
        // Since we are throwing the object, then our forceMode will be Impulse. This means that the force to add will
        // be our momentum since we want the velocity to change instantly. Thus, our force vector is p, and we 
        // can calculate our velocity.
        if (mass)
        {
            return force / _pickedUpObjectRigidbody.mass;
        }
        //Debug.Log(forceDir);
        return force;
    }

    private void ThrowItem()
    {
        Vector3 force = CalculateVelocity();
        //Debug.Log("Throw!");
        GameObject item = _pickedUpObject;
        // First, drop the object
        DropItem();
        // We want to throw object in direction the player is facing
        // This should be the transform.forward of our player object.
        // We need to apply an impulse force in the respective direction

        // float angle = throwAngle * Mathf.Deg2Rad;
        // Vector3 angleDirection = new Vector3(0, Mathf.Sin(angle), 0f);
        // Vector3 forceDirection = (transform.forward + angleDirection).normalized * throwForce;
        // //Debug.Log(forceDirection)
        //Debug.Log(force);
        item.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        
        
        // Add Component to the package to alert enemies on colliding with ground.
        item.AddComponent<OnCollisionEvent>();
        

    }

    private void DropDolly()
    {
        // Dont drop dolly if we have an item in it!
        if (_hasItem) return;

        // Re-enable collider.
        dolly.GetComponent<Collider>().enabled = true;
        dolly.GetComponent<Collider>().attachedRigidbody.isKinematic = false;
        dolly.GetComponent<Collider>().attachedRigidbody.useGravity = true;
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
        item.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        // Set parent to be player
        item.transform.SetParent(gameObject.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.transform.GetComponent<Rigidbody>().freezeRotation = true;
        item.transform.localEulerAngles = Vector3.zero;
        dolly = item;
        _hasDolly = true;
        
        // Disable collider so that it ignores all collision when
        // we pick it up.
        Collider col = item.GetComponent<Collider>();
        col.attachedRigidbody.useGravity = false;
        col.attachedRigidbody.isKinematic = true;
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
        _pickedUpObjectRigidbody = null;
        _hasItem = false;
        
        // Reset our trajectory calculations
        _currentForceMultiplier = 0f;
        // Disable LineRenderer
        lineRenderer.enabled = false;
        
        

        //Debug.Log("Dropped Item");
    }

    private void PickupItem()
    {
        
        GameObject item = GetItem();
        if (item == null) return;
        if (!item.CompareTag("Package") && !item.CompareTag("GoldenPackage")) return;
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
        Collider col = item.GetComponent<Collider>();
        col.enabled = false;
        // Ignore collisions between player and package

        _hasItem = true;
        _pickedUpObject = item;
        _pickedUpObjectRigidbody = item.GetComponent<Rigidbody>();
    }

    public GameObject GetItem()
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
