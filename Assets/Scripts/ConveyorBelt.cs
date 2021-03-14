using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 pos = _rigidbody.position;
        _rigidbody.position += transform.forward * (moveSpeed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(pos);
    }
}
