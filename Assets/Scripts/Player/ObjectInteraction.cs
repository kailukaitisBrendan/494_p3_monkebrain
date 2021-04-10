using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using System.Linq;

public class ObjectInteraction : MonoBehaviour
{
    public float pickupDistance;
    //public float pickupFront = 0.5f;
    public float pickupHeight = 0.5f;
    public Vector3 pickupBox;

    public Transform itemSlot;
    public Transform itemSlot2;
    public GameObject dolly;

    public LineRenderer lineRenderer;

    public float throwForce;
    public float maxForceMultiplier;
    public float throwAngle = 45f;

    public float chargeForceScaler = 1.5f;
    public UnityEvent onThrow;

    private float action_delay = 0.2f;
    private float _actionT = 0.0f;

    private float _numItems = 0;
    private bool _hasDolly = false;
    private Stack<GameObject> _pickedUpObjects = new Stack<GameObject>();
    private float _currentForceMultiplier = 0.0f;
    private Rigidbody _pickedUpObjectRigidbody;
    private BoxCollider _opc;

    private bool _eventInvoked = false;

    // public Image box1;
    // public Image box2;
    private bool _inputHeld = false;
    private float _pressedTime = 0f;
    private GameObject holdingBox;
    private GameObject notHoldingBox;



    //holobox stuff
    Vector3 holoboxPos;
    Vector3 halfExtents = new Vector3(0.5f, 0.5f, 0.5f);
    GameObject holoBox;




    private void Start()
    {
        // box2.enabled = false;
        _opc = GetComponentInChildren<BoxCollider>();
        holdingBox = transform.Find("holdingBox").gameObject;
        notHoldingBox = transform.Find("notHoldingBox").gameObject;
        holoBox = transform.Find("Holobox").gameObject;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        holoBox.SetActive(false);
    }

    private void Update()
    {
        //BoxColorChange();

        //Add box collider if has dolly
        if (_hasDolly || _numItems > 0)
        {
            _opc.enabled = true;
            holdingBox.SetActive(true);
            notHoldingBox.SetActive(false);
        }
        else
        {
            _opc.enabled = false;
            holdingBox.SetActive(false);
            notHoldingBox.SetActive(true);
        }


        if (Input.GetMouseButtonDown(0) &&
            Time.time - _actionT > action_delay)
        {
            _actionT = Time.time;
            if (!_hasDolly)
            {
                PickupDolly();
                if (_hasDolly)
                    return;
            }


            //action_t = Time.time;
            if (_numItems == 0)
            {
                // if no item, pick up item.

                PickupItem(itemSlot);
            }
            else if (_numItems == 1 && _hasDolly)
            {
                PickupItem(itemSlot2);
            }
        }

        if (Input.GetMouseButton(1) &&
            Time.time - _actionT > action_delay)
        {
            _actionT = Time.time;
            if (_numItems == 0 && _hasDolly)
            {
                DropDolly();
                return;
            }
            else if (_hasDolly)
            {
                DropItem();
                return;
            }
        }

        // Cancel out of throw
        if (Input.GetMouseButtonDown(0) && lineRenderer.enabled) {
            // Reset our trajectory calculations
            _currentForceMultiplier = 0f;
            // Disable LineRenderer
            lineRenderer.enabled = false;
            holoBox.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            _pressedTime = Time.timeSinceLevelLoad;
            _inputHeld = false;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //Debug.Log("Mouse Up!");
            if (!_inputHeld)
            {
                // Player released right click without holding, so drop item
                //Debug.Log("Right Click!");
                DropItem();
            }
            else if (_inputHeld && _currentForceMultiplier > 0.0f)
            {
                //Debug.Log("Right Click and Hold!");
                // Input was held, so throw item
                if (_numItems == 0 || _hasDolly) return;
                ThrowItem();
                onThrow.Invoke();
                _eventInvoked = false;
                _currentForceMultiplier = 0.0f;
            }

            _inputHeld = false;
        }

        if (Input.GetMouseButton(1) && Time.timeSinceLevelLoad - _pressedTime > action_delay)
        {
            //Debug.Log("Holding!");
            // Holding down the button.
            _inputHeld = true;
            // charge up the throw
            if (_numItems == 1 && !_hasDolly)
            {
                if (!_eventInvoked)
                {
                    onThrow.Invoke();
                    EventBus.Publish(new ThrowingEvent());
                    _eventInvoked = true;
                }

                ChargeThrow();
            }
        }
    }


    private void ChargeThrow()
    {
        _currentForceMultiplier += Time.deltaTime * chargeForceScaler;

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
        holoBox.SetActive(true);
        // ---- Draw trajectory path -----
        // To draw the trajectory path we need to simulate the projectile position across set intervals.
        // First, we need to calculate the total time the projectile will take before landing
        // This is given by the equation t = (vsin(θ) + sqr((vsin(θ)^2 + 2gy_0)) / g where
        // θ = Angle of the projectile
        // y_0 = the initial height of the projectile
        // v = the magnitude of the velocity vector
        // g = gravity

        Vector3 position = _pickedUpObjects.Peek().transform.position;
        Vector3 velocity = CalculateVelocity(true);
        //Debug.Log(velocity);
        float v = velocity.magnitude;
        // Calculate magnitude
        // Since Physics.gravity.y returns a negative value, we have to convert to absolute value. 
        float totalTime = (v * Mathf.Sin(throwAngle) +
                           Mathf.Sqrt(Mathf.Pow(v * Mathf.Sin(throwAngle), 2) +
                                      Mathf.Abs(2 * Physics.gravity.y * position.y)));
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

        foreach (var item in path)
        {
            
            if (HoloBoxOverLap(item))
            {
                holoboxPos = item;
               
                break;
            }
        }
        holoBox.transform.position = holoboxPos;
        
        
        
    }

   
    private bool HoloBoxOverLap(Vector3 point)
    {
        LayerMask mask = LayerMask.GetMask("Player") + LayerMask.GetMask("ObjectPickedUp");
        if (Physics.OverlapBox(point, halfExtents, transform.rotation, ~mask).Length > 0)
        
        {
           // foreach (var item in Physics.OverlapBox(point, halfExtents, transform.rotation, ~mask))
            //{
              //  Debug.Log(item.name);
            //}
            return true;
        }
        return false;
    }



    private Vector3 CalculateVelocity(bool mass = false)
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
        GameObject item = _pickedUpObjects.Peek();
        // First, drop the object
        if (!item.CompareTag("BluePackage"))
        {
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
            //item.AddComponent<OnCollisionEvent>();
            OnCollisionEvent collisionEvent = item.AddComponent<OnCollisionEvent>();
            // Set the collision event destroyOnCollision flag
            collisionEvent.destroyOnCollision = true;
        }
        else
        {
            // Reset our trajectory calculations
            _currentForceMultiplier = 0f;
            // Disable LineRenderer
            lineRenderer.enabled = false;
            holoBox.SetActive(false);


            // Add Force to player
            Rigidbody rb = GetComponent<Rigidbody>();
            // First add vertical force
            rb.AddForce(force, ForceMode.Impulse);

            ThirdPersonMovement tpm = GetComponent<ThirdPersonMovement>();
            tpm.baseVelocity = new Vector3(force.x, 0f, force.z);
        }
        EventBus.Publish(new ThrowingEvent());
    }

    private void DropDolly()
    {
        // Dont drop dolly if we have an item in it!
        if (_numItems > 0) return;

        // Debug.Log("drop called");

        // Re-enable collider.
        dolly.GetComponent<Collider>().enabled = true;
        dolly.GetComponent<Collider>().attachedRigidbody.isKinematic = false;
        dolly.GetComponent<Collider>().attachedRigidbody.useGravity = true;

        // Subtract dolly mass from player
        GetComponent<Rigidbody>().mass -= dolly.GetComponent<Collider>().attachedRigidbody.mass;

        dolly.transform.parent = null;
        dolly = null;
        _hasDolly = false;
    }

    private void PickupDolly()
    {
        GameObject item = GetDolly();
        if (item == null) return;

        if (!item.CompareTag("Dolly")) return;
        Rigidbody rb = item.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        // Set parent to be player
        item.transform.SetParent(gameObject.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        rb.freezeRotation = true;
        item.transform.localEulerAngles = Vector3.zero;
        dolly = item;
        _hasDolly = true;

        // Disable collider so that it ignores all collision when
        // we pick it up.
        Collider col = item.GetComponent<Collider>();
        col.attachedRigidbody.useGravity = false;
        col.attachedRigidbody.isKinematic = true;
        col.enabled = false;

        // Add dolly mass to player
        GetComponent<Rigidbody>().mass += rb.mass;
    }

    private bool CanDrop()
    {
        LayerMask mask = LayerMask.GetMask("Player") + LayerMask.GetMask("ObjectPickedUp");

        if (Physics.OverlapBox(itemSlot.position, halfExtents, transform.rotation, ~mask).Length > 0)
        {
            return false;
        }
        return true;

    }
    private void DropItem()
    {
        if (_pickedUpObjects.Count == 0) return;
        if (!CanDrop()) return;

        GameObject item = _pickedUpObjects.Peek();

        if (_hasDolly && _numItems == 2)
        {
            item.transform.position += transform.forward;
        }
        
        _numItems--;
        LayerMask mask = LayerMask.GetMask("Grabbable Object");
        float dist = 2f;
        Debug.DrawRay(transform.position, transform.forward * dist);
        if (Physics.Raycast(transform.position, transform.forward, dist, mask))
        {
            item.transform.position += Vector3.up;
        }

        // Re-enable rigidbody and collider.
        Rigidbody rb = item.GetComponent<Rigidbody>();

        Collider col = item.GetComponent<Collider>();

        // Set the parent back to null
        item.transform.parent = null;
        rb.isKinematic = false;
        col.enabled = true;

        _pickedUpObjectRigidbody = item.GetComponent<Rigidbody>();
        _pickedUpObjects.Pop();

        // Subtract object mass from player
        //GetComponent<Rigidbody>().mass -= rb.mass;

        // Reset our trajectory calculations
        _currentForceMultiplier = 0f;
        // Disable LineRenderer
        lineRenderer.enabled = false;
        holoBox.SetActive(false);
        EventBus.Publish(new ObjectInteractionEvent());
    }


    private void PickupItem(Transform itemSlot)
    {
        GameObject item = GetItem();
        if (item == null) return;
        _numItems++;

        string[] validTags = {"Package", "GoldenPackage", "BluePackage"};

        // Return if item does not have a valid tag
        if (!validTags.Any(tag => item.CompareTag(tag))) return;
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

        // Add object mass to player
        //GetComponent<Rigidbody>().mass += rb.mass;

        // Ignore collisions between player and package

        _pickedUpObjects.Push(item);
        _pickedUpObjectRigidbody = item.GetComponent<Rigidbody>();
        EventBus.Publish(new ObjectInteractionEvent());
    }


    public GameObject GetItem()
    {
        // Raycast out from player to see if item is in front of the player.
        // Create mask so we only collide with pickup-able objects.
        LayerMask mask = LayerMask.GetMask("Grabbable Object") + LayerMask.GetMask("Golden Package");
        Vector3 pos = transform.position;
        pos.y = itemSlot.position.y - pickupHeight;
        RaycastHit hit;
        if (Physics.BoxCast(pos, pickupBox, Vector3.forward * pickupBox.z, out hit, Quaternion.identity, pickupDistance / 2,
                mask)
            || Physics.Raycast(pos, transform.TransformDirection(Vector3.forward), out hit, pickupDistance, mask))
        {
            Debug.DrawRay(pos, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
        return hit.transform == null ? null : hit.transform.gameObject;
    }

    public GameObject GetDolly()
    {
        // Raycast out from player to see if item is in front of the player.
        // Create mask so we only collide with pickup-able objects.
        LayerMask mask = LayerMask.GetMask("Dolly");
        Vector3 pos = transform.position;
        pos.y = itemSlot.position.y;
        RaycastHit hit;
        float radius = 1f;
        if (Physics.SphereCast(pos, radius, transform.TransformDirection(Vector3.forward), out hit, pickupDistance / 2,
                mask)
            || Physics.Raycast(pos, transform.TransformDirection(Vector3.forward), out hit, pickupDistance, mask))
        {
            Debug.DrawRay(pos, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }

        return hit.transform == null ? null : hit.transform.gameObject;
    }

    //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
       // Vector3 pos = transform.position;
        //pos.z = itemSlot.position.z + pickupFront;
        //pos.y = itemSlot.position.y - pickupHeight;
        //Gizmos.color = Color.red;

        //Draw a Ray forward from GameObject toward the maximum distance
        //  Gizmos.DrawRay(pos, Vector3.forward * pickupBox.z * pickupDistance / 2);
        //Draw a cube at the maximum distance
        // Gizmos.DrawWireCube(pos + Vector3.forward * pickupBox.z * pickupDistance / 2, transform.localScale);


        //holobox checking
        Gizmos.color = Color.blue;
        //Draw a cube that extends to where the hit exists
        Gizmos.DrawWireCube(holoboxPos, halfExtents);
    }
}