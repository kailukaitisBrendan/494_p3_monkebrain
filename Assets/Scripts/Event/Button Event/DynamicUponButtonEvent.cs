using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicUponButtonEvent : MonoBehaviour
{
    private Subscription<ButtonPressEvent> buttonPressSubscription;
    private Subscription<ButtonLiftEvent> buttonLiftSubscription;

    public int buttonEventId;
    public bool reversible;
    private float lerpSpeed = 0.0002f;
    float i = 0;
    
    // Variables representing original state
    private bool isKinematic = true;
    private Vector3 initPosition;
    private Quaternion initRotation;

    private Vector3 currPosition;
    private Quaternion currRotation;

    // Start is called before the first frame update
    void Start()
    {
        buttonPressSubscription = EventBus.Subscribe<ButtonPressEvent>(MakeRigidbodyDynamic);
        
        if (reversible) {
            buttonLiftSubscription = EventBus.Subscribe<ButtonLiftEvent>(MakeRigidbodyKinematic);
            
            initPosition = transform.position;
            initRotation = transform.rotation;
        }
    }

    private void MakeRigidbodyDynamic(ButtonPressEvent _event)
    {
        if (_event.id == buttonEventId) {
            GetComponent<Rigidbody>().isKinematic = false;
            isKinematic = false;
        }
    }

    private void MakeRigidbodyKinematic(ButtonLiftEvent _event)
    {
        if (_event.id == buttonEventId) {
            GetComponent<Rigidbody>().isKinematic = true;
            isKinematic = true;
        }
    }
    
    private void FixedUpdate() {
        if (isKinematic && reversible) {
            currPosition = transform.position;
            currRotation = transform.rotation;
            transform.position = Vector3.Lerp(currPosition, initPosition, i * lerpSpeed);
            transform.rotation = Quaternion.Lerp(currRotation, initRotation, i * lerpSpeed);
            i++;
            if (i > 100) {
                i = 0;
            }
            if (Mathf.Abs((initPosition.x-currPosition.x) 
                + (initPosition.y-currPosition.y) 
                + (initPosition.z-currPosition.z)) < 0.001) {
                isKinematic = false;
            }
        }
    }
}
