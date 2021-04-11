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

    AudioSource sound;
    void Start()
    {
        initialPosition = transform.localPosition.y;
        sound = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // Publish button press event when a button is pressed beyond it's threshold
        if (!pressed && 
            transform.localPosition.y / initialPosition <= threshold) {
            Press();
        }
        else if (pressed && transform.localPosition.y / initialPosition > threshold) {
            Lift();
        }
    }

    public void Press()
    {
        EventBus.Publish<ButtonPressEvent>(new ButtonPressEvent(buttonEventId));
        sound.Play();
        pressed = true;

        Debug.Log("Button " + buttonEventId + " pressed");
    }

    public void Lift()
    {
        EventBus.Publish<ButtonLiftEvent>(new ButtonLiftEvent(buttonEventId));
        pressed = false;

        Debug.Log("Button " + buttonEventId + " lifted");

    }
}
