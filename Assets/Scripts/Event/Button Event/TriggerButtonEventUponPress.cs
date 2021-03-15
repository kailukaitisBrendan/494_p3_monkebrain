using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButtonEventUponPress : MonoBehaviour
{
    // Proportion of initial button distance to be compressed
    //  to qualify for a button press event

    [Range(0, 1)] 
    public float threshold;
    private float initialPosition;

    public int buttonEventId;
    private bool pressed = false;

    void Start()
    {
        initialPosition = transform.localPosition.y;
    }

    void FixedUpdate()
    {
        // Publish button press event when a button is pressed beyond it's threshold
        if (!pressed && 
            transform.localPosition.y / initialPosition <= threshold) {
            
            EventBus.Publish<ButtonPressEvent>(new ButtonPressEvent(buttonEventId));
            pressed = true;
        }
        else if (transform.localPosition.y / initialPosition > threshold) {
            
            EventBus.Publish<ButtonLiftEvent>(new ButtonLiftEvent(buttonEventId));
            pressed = false;
        }
    }
}
