using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class OnCollisionEvent : MonoBehaviour
{
    public LayerMask collisionLayers;
    public List<string> collisionTags;

    public bool destroyOnCollision = false;
    //public HitObjectEvent onObjectHit;
    private int count = 0;

    private void Start()
    {
        // Default layer mask to ground and enemies
        collisionLayers = LayerMask.GetMask("Ground", "Enemy", "Conveyor Belt", "Iteractable Object");
        collisionTags = new List<string> {"Enemy"};
    }

    private void OnCollisionEnter(Collision other)
    {
        // Return if just respawned
        bool justRespawned = false;
        Respawnable resp = GetComponent<Respawnable>();
        if (resp) {
            justRespawned = resp.JustRespawned();
        }

        // Check if we hit a tag first
        if (collisionTags != null && collisionTags.Contains(other.gameObject.tag))
        {
            Debug.Log("Collided with object! " + other.gameObject.name + count);
            EventBus.Publish(new HitObjectEvent(gameObject, other.gameObject, justRespawned));
            
            // Remove the component.
            if (destroyOnCollision)
            {
                Destroy(this);
            }

            count++;
            return;
        }
        // Check if we collided with an object in the chosen layers.
        int layer = other.gameObject.layer;
        if (collisionLayers == (collisionLayers | (1 << layer)))
        {
            Debug.Log("Collided with object! " + other.gameObject.name + count);
            EventBus.Publish(new HitObjectEvent(gameObject, other.gameObject, justRespawned));
            
            // Remove the component.
            if (destroyOnCollision)
            {
                Destroy(this);
            }

            count++;
        }

    }
}
