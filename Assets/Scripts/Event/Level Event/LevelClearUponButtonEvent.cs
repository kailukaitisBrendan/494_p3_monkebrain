using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClearUponButtonEvent : MonoBehaviour
{
    private Subscription<ButtonPressEvent> buttonPressSubscription;

    private bool buttonPressed = false;
    public int buttonPressEventId;

    // Start is called before the first frame update
    void Start()
    {
        buttonPressSubscription = EventBus.Subscribe<ButtonPressEvent>(TriggerButtonPressed);
        EventBus.Publish<AddWinConEvent>(new AddWinConEvent(GetButtonPressed));
    }

    private bool GetButtonPressed()
    {
        return buttonPressed;
    }

    private void TriggerButtonPressed(ButtonPressEvent _event)
    {
        if (_event.id == buttonPressEventId) {
            buttonPressed = true;
        }
    }

}
