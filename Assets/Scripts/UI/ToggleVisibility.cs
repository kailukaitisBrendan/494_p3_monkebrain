using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVisibility : MonoBehaviour
{
    private Subscription<ObjectInteractionEvent> _toggleVisibilitySubscription;
    // Start is called before the first frame update
    private Image _image;
    void Start()
    {
        _toggleVisibilitySubscription = EventBus.Subscribe<ObjectInteractionEvent>(OnToggleVisibility);
        _image = GetComponent<Image>();
    }


    void OnToggleVisibility(ObjectInteractionEvent e)
    {
        // Change the gameObject's color
        Color newColor = _image.color;
        newColor.a = _image.color.a == 1.0f ? 0.2f : 1f;
        _image.color = newColor;
    }
}
