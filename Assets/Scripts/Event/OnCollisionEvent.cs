﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class OnCollisionEvent : MonoBehaviour
{
    public LayerMask collisionLayers;

    public bool destroyOnCollision = false;
    //public HitObjectEvent onObjectHit;

    private void Start()
    {
        // Default layer mask to ground and enemies
        collisionLayers = LayerMask.GetMask("Ground", "Enemy", "Conveyor Belt");
    }

    private void OnCollisionEnter(Collision other)
    {
        // Check if we collided with an object in the chosen layers.
        int layer = other.gameObject.layer;
        if (collisionLayers == (collisionLayers | (1 << layer)))
        {
            //Debug.Log("Collided with object!");
            EventBus.Publish<HitObjectEvent>(new HitObjectEvent(gameObject, other.gameObject));
            
            // Remove the component.
            if (destroyOnCollision)
            {
                Destroy(this);
            }
        }
        
        
    }
}
