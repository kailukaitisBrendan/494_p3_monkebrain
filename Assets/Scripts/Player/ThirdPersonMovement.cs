using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Numerics;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ThirdPersonMovement : MonoBehaviour
{
    private Rigidbody _rb;
    public float movementSpeed = 6f;
    public float rotationSpeed = 10f;
    public float jumpPower = 5f;

    public LayerMask groundMask;

    public Transform groundCheck;

    public float groundDistance = 0.4f;

    public AudioClip hitGround;
    
    private float _angleVelocity;
    private bool _isGrounded = false;
    private bool _isThrowing = false;
    private Camera _mainCamera;
    private bool _isPlayingWalkingSound = false;
    private AudioSource _sound;

    private bool _jumped = false;

    // Denotes base velocity (before factoring in player inputs)
    public Transform followTransform;
    public AxisState xAxis;
    public AxisState yAxis;

    public Vector3 baseVelocity;

    // maybe fix for input?
    Vector3 velocity;
    Vector3 direction;

    private void Awake()
    {
        // Lock the cursor to the game window
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
        direction = Vector3.zero;
        _rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
        _mainCamera = Camera.main;
        _sound = GetComponent<AudioSource>();
    }

    

    // Update is called once per frame
    void Update()
    {
        // Try and align player's rotation with the angle of the ground. 
        //AlignWithGround();

        // Get the movement inputs.
        velocity = Vector3.zero;
        _isGrounded = IsGrounded();
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")).normalized;


        // THROWING
        if (_isThrowing)
        {
            xAxis.Update(Time.deltaTime);
            yAxis.Update(Time.deltaTime);

            followTransform.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
            
            // Rotate the player to match the camera yaw
            float yawCamera = _mainCamera.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), rotationSpeed * Time.deltaTime);
        }

        if (direction.magnitude >= 0.1f)
        {
            // play walking sound
             if (_isGrounded && !_isPlayingWalkingSound)
             {
                 // _sound.Stop();
                 // _sound.loop = true;
                 // _sound.clip = walkingSound;
                 // _sound.Play();
                 _isPlayingWalkingSound = true;
             }


            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                _mainCamera.transform.eulerAngles.y;
            if (!_isThrowing)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, targetAngle, 0f),
                    Time.deltaTime * rotationSpeed);
            }

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            velocity = moveDir.normalized * movementSpeed;
        }
        else
        {
            //stop walking sound
            // if (_sound.clip != hitGround)
            //     _sound.Stop();
            _isPlayingWalkingSound = false;
            velocity = Vector3.zero;
        }

        // JUMPING
        velocity.y = _rb.velocity.y;
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _isGrounded = false;
            velocity.y = jumpPower;
            _rb.velocity = velocity + baseVelocity;
        }
        if (_isGrounded)
        {
            baseVelocity = Vector3.zero;
        }
        if (_jumped && _isGrounded)
        {
            //_sound.clip = hitGround;
            _sound.loop = false;
            _sound.PlayOneShot(hitGround);
            _jumped = false;
            StartCoroutine(WaitForFallSoundToFinish());
        }
        //stop walking sound
        if (!_isGrounded)
        {
            _jumped = true;
            // _sound.Stop();
            _isPlayingWalkingSound = false;
        }

        //Animation publisher
        EventBus.Publish<MovementEvent>(new MovementEvent(_isPlayingWalkingSound, _jumped, _isGrounded));
    }

    void FixedUpdate() {
        _rb.velocity = velocity + baseVelocity;
    }


    IEnumerator WaitForFallSoundToFinish()
    {
        yield return new WaitForSeconds(0.3f);
        _isPlayingWalkingSound = false;
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    private void AlignWithGround()
    {
        // This function tries to align the player's rotation with the angle of the ground if they are on a slope.
        RaycastHit hitInfo;
        Debug.DrawRay(groundCheck.position, Vector3.down * 2f, Color.cyan);
        if (Physics.Raycast(groundCheck.position, Vector3.down, out hitInfo, 1f, groundMask))
        {
            // We hit the ground, calculate the angle of the player and our hit's normal.
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if (angle < 30)
            {
                Vector3 slopeForward = Vector3.Cross(transform.right, hitInfo.normal);
                Quaternion lookRotation = Quaternion.LookRotation(slopeForward);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
                _rb.freezeRotation = true;
            }
            else
            {
                _rb.freezeRotation = false;
            }
        }
    }

    public void OnToggleThrowing()
    {
        _isThrowing = !_isThrowing;
    }
}