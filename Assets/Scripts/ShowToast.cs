using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowToast : MonoBehaviour
{
    private Subscription<ObjectInteractionEvent> _pickupEventSub;
    public bool throwToast;
    public bool pickupToast;
    public bool dropToast;
    public bool buttonToast;
    public bool enemyToast;
    public bool enemyDazedToast;
    public bool enemyDistractedToast;
    public bool bossToast;
    public bool doToasts;

    bool toastTriggered1 = false;
    bool toastTriggered2 = false;

    // added some audio to the enemy warning toast
    public AudioSource watch_out;
    public AudioSource stuffalo_steal;

    // Start is called before the first frame update
    void Start()
    {
        if (!doToasts) return;

        _pickupEventSub = EventBus.Subscribe<ObjectInteractionEvent>(_OnPickup);

        if (!toastTriggered1)
        {
            if (pickupToast)
            {
                EventBus.Publish<ToastRequestEvent>(new ToastRequestEvent("Left click to pick up the package"));
            }
            toastTriggered1 = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void _OnPickup(ObjectInteractionEvent e)
    {
        string message = "";
        if (!toastTriggered2)
        {
            if (throwToast)
            {
                message = "Hold right click to throw the package";
            }
            
            if (dropToast)
            {
                message = "Right click to drop the package";
            }
            if (buttonToast)
            {
                message = "Drop the package on a button to trigger bridges";
            }
            if (enemyToast)
            {
                message = "Watch out for bandits!";
                if (watch_out != null)
                    watch_out.Play();
            }
            if (enemyDistractedToast)
            {
                message = "Throw a package near an enemy to distract them";
            }
            if (enemyDazedToast)
            {
                message = "Throw and hit an enemy with a package to daze them";
            }
            if (bossToast)
            {
                message = "Defeat Stuffalo Steal by sending some TNT his way!";
                if (watch_out != null)
                    stuffalo_steal.Play();
            }
            toastTriggered2 = true;
            EventBus.Publish<ToastRequestEvent>(new ToastRequestEvent(message));
        }
    }
}
