using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUponButtonEvent : MonoBehaviour
{
    private Subscription<ButtonPressEvent> buttonPressSubscription;
    private Subscription<ButtonLiftEvent> buttonLiftSubscription;

    public int buttonEventId;
    public bool reversible;
    public Vector3 positionShift;

    // Private variables
    private float moveSpeed = 0.3f;
    private float returnSpeed = 0.05f;
    private bool isShifted;
    private Vector3 initPosition;

    


    AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        isShifted = false;
        initPosition = transform.position;

        buttonPressSubscription = EventBus.Subscribe<ButtonPressEvent>(ButtonPressHandler);

        if (reversible) {
            buttonLiftSubscription = EventBus.Subscribe<ButtonLiftEvent>(ButtonLiftHandler);
        }
    }

    private void ButtonPressHandler(ButtonPressEvent _event)
    {
        if (_event.id == buttonEventId) {
            isShifted = true;
            sound.Play();
        }
    }

    private void ButtonLiftHandler(ButtonLiftEvent _event)
    {
        if (_event.id == buttonEventId) {
            isShifted = false;
            sound.Play();
        }
    }
    
    private void FixedUpdate() {
        Vector3 currPosition = transform.position;
        if (isShifted) {
            transform.position = Vector3.MoveTowards(currPosition, initPosition + positionShift, moveSpeed);
        }
        else if (!isShifted) {
            transform.position = Vector3.MoveTowards(currPosition, initPosition, returnSpeed);
        }
    }
}
