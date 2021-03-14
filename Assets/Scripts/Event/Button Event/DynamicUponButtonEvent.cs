using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicUponButtonEvent : MonoBehaviour
{
    private Subscription<ButtonPressEvent> buttonPressSubscription;
    public int buttonPressEventId;

    // Start is called before the first frame update
    void Start()
    {
        buttonPressSubscription = EventBus.Subscribe<ButtonPressEvent>(MakeRigidbodyDynamic);
    }

    void MakeRigidbodyDynamic(ButtonPressEvent _event)
    {
        if (_event.id == buttonPressEventId) {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

}
