using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSoundOnEvents : MonoBehaviour
{
    Subscription<PlayerSpottedEvent> playerSpottedSubscription;
    Subscription<HitObjectEvent> hitObjectSubscription;

    AudioSource sound;
    public AudioClip playerSpotted;
    public AudioClip hitObjectSound;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
        playerSpottedSubscription = EventBus.Subscribe<PlayerSpottedEvent>(OnPlayerSpotted);
        hitObjectSubscription = EventBus.Subscribe<HitObjectEvent>(OnHitObject);
    }

    void OnPlayerSpotted(PlayerSpottedEvent e)
    {
        if (e.player != null)
        {
            
            sound.clip = playerSpotted;
            sound.Play();
        }
    }
    void OnHitObject(HitObjectEvent e)
    {
        //if hit enemy
        if (e.hitObject.layer == 13)
        {
            sound.clip = hitObjectSound;
            sound.Play();
        }
    }
    

    
}
