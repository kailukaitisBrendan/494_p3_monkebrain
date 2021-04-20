using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassRaiseMovement : MonoBehaviour
{
    public GameObject hand;

    private Vector3 _offset;

    private void Start()
    {
        // Calculate offset
        _offset = transform.position - hand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       // Set the object's position to be the hand's position plus the offset
       Vector3 newPosition = hand.transform.position + _offset;

       transform.position = newPosition;
    }

    // IEnumerator WaitFalse()
    // {
    //     yield return new WaitForSeconds(0.1f);
    //     goingUp = false;
    // }
    //
    // IEnumerator WaitTrue()
    // {
    //     yield return new WaitForSeconds(0.1f);
    //     goingUp = true;
    // }
}

