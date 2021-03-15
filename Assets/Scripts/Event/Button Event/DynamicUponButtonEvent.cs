using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicUponButtonEvent : MonoBehaviour
{
    private Subscription<ButtonPressEvent> buttonPressSubscription;
    private Subscription<ButtonLiftEvent> buttonLiftSubscription;

    public int buttonEventId;
    public bool reversible;
    public float lerpSpeed = 0.01f;
    
    // Variables representing original state
    private bool isKinematic = true;
    private Vector3 initPosition;
    private Quaternion initRotation;

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

            StartCoroutine(LerpToOriginalState());
        }
    }

    private IEnumerator LerpToOriginalState() 
    {
        Vector3 currPosition = transform.position;
        Quaternion currRotation = transform.rotation;

        for (int i = 0; i < 100 && isKinematic; ++i) {
            transform.position = Vector3.Lerp(currPosition, initPosition, i * lerpSpeed);
            transform.rotation = Quaternion.Lerp(currRotation, initRotation, i * lerpSpeed);
            yield return null;
        }
    }

}
