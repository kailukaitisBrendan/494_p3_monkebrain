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
    private delegate void Handler();
    private Handler pressHandler;
    private Handler liftHandler; 

    private Vector3 initPosition;
    private Quaternion initRotation;

    private Vector3 currPosition;
    private Quaternion currRotation;

    AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        isKinematic = GetComponent<Rigidbody>().isKinematic;
        if (isKinematic) {
            pressHandler = MakeRigidbodyDynamic;
            liftHandler = MakeRigidbodyKinematic;
        }
        else {
            pressHandler = MakeRigidbodyKinematic;
            liftHandler = MakeRigidbodyDynamic;
        }

        initPosition = transform.position;
        initRotation = transform.rotation;

        buttonPressSubscription = EventBus.Subscribe<ButtonPressEvent>(ButtonPressHandler);

        if (reversible) {
            buttonLiftSubscription = EventBus.Subscribe<ButtonLiftEvent>(ButtonLiftHandler);
        }
    }

    private void ButtonPressHandler(ButtonPressEvent _event)
    {
        if (_event.id == buttonEventId && pressHandler != null) {
            pressHandler();
            sound.Play();
        }
    }

    private void ButtonLiftHandler(ButtonLiftEvent _event)
    {
        if (_event.id == buttonEventId && liftHandler != null) {
            liftHandler();
            sound.Play();
        }
    }

    private void MakeRigidbodyDynamic()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        isKinematic = false;
    }

    private void MakeRigidbodyKinematic()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        isKinematic = true;
    }
    
    private void FixedUpdate() {
        if (isKinematic) {
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
