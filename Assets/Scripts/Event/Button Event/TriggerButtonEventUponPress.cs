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

    public int buttonPressEventId;

    void Start()
    {
        initialPosition = transform.localPosition.y;
    }

    void Update()
    {
        if (transform.localPosition.y / initialPosition <= threshold) {
            EventBus.Publish<ButtonPressEvent>(new ButtonPressEvent(buttonPressEventId));
        }
    }
}
