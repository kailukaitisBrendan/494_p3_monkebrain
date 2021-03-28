﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    Rigidbody rb;
    public float movementSpeed = 6f;
    public float jumpPower = 5f;

    public LayerMask groundMask;

    public Transform groundCheck;

    public float groundDistance = 0.4f;
    //public float climbSpeed = 3f;

    public float angleDamping = 0.1f;
    //public LayerMask Climbable;

    public Transform cam;
    
    private float _angleVelocity;
    private bool _isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        AlignWithGround();
        
        Vector3 velocity = Vector3.zero;
        _isGrounded = IsGrounded();
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _angleVelocity, angleDamping);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            velocity = moveDir.normalized * movementSpeed;
            //Debug.Log(velocity);
        }
        else
        {
            velocity = Vector3.zero;
        }
        
        velocity.y = rb.velocity.y;

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _isGrounded = false;
            velocity.y = jumpPower;
        }

        rb.velocity = velocity;
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    private void AlignWithGround()
    {
        // This function tries to align the player's rotation with the angle of the ground if they are on a slope.
        RaycastHit hitInfo;
        if (Physics.Raycast(groundCheck.position, Vector3.down, out hitInfo, 1f, groundMask))
        {
            // We hit the ground, calculate the angle of the player and our hit's normal.
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if (angle < 30)
            {
                Vector3 slopeForward = Vector3.Cross(transform.right, hitInfo.normal);
                Quaternion lookRotation = Quaternion.LookRotation(slopeForward);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 10 * Time.deltaTime );
                rb.freezeRotation = true;
            }
            else
            {
                rb.freezeRotation = false;
            }
        }

    }
}
