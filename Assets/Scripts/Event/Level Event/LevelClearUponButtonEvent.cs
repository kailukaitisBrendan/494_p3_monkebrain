using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClearUponButtonEvent : MonoBehaviour
{
    private Subscription<ButtonPressEvent> buttonPressSubscription;

    private bool buttonPressed = false;
    
    // Golden package condition variables
    public bool onlyWhenHoldingGoldenPackage = true;
    private float thresholdRadius = 1;

    public int buttonPressEventId;

    private GameObject goldenPackage;

    // Start is called before the first frame update
    void Start()
    {
        buttonPressSubscription = EventBus.Subscribe<ButtonPressEvent>(TriggerButtonPressed);
        EventBus.Publish<AddWinConEvent>(new AddWinConEvent(GetButtonPressed));
        goldenPackage = GameObject.FindGameObjectWithTag("GoldenPackage");
    }

    private bool GetButtonPressed()
    {
        if (onlyWhenHoldingGoldenPackage && goldenPackage) {
            return (goldenPackage.transform.position - transform.position).sqrMagnitude <= thresholdRadius * thresholdRadius;
        }
        else if (!onlyWhenHoldingGoldenPackage) {
            return buttonPressed;
        }
        return false;
    }

    private void TriggerButtonPressed(ButtonPressEvent _event)
    {
        if (_event.id == buttonPressEventId) {
            buttonPressed = true;
        }
    }

}
