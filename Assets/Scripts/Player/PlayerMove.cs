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

public class PlayerMove : MonoBehaviour
{
    public ParticleSystem dust;
    private Rigidbody _rb;
    public float baseMovementSpeed = 1f;
    public float rotationSpeed = 10f;

    public LayerMask groundMask;

    public Transform groundCheck;

    public float groundDistance = 0.4f;

    public AudioClip hitGround;
    
    private float _angleVelocity;
    public bool _isGrounded = false;
    private bool _isThrowing = false;
    private bool _firstThrow = false;
    private Camera _mainCamera;
    private bool _isPlayingWalkingSound = false;
    private AudioSource _sound;
    private float _movementSpeed = 0.2f;

    private bool _jumped = false;

    // Denotes base velocity (before factoring in player inputs)
    public Transform followTransform;
    public AxisState xAxis;
    public AxisState yAxis;

    // maybe fix for input?
    Vector3 velocity;
    Vector3 direction;
    CharacterController controller;
    public float gravity = -0.05f;
    float time_jump = 0f;
    public float jumpPower = 0.3f;
    public float jumpAppliedTime = 0.2f;

    float groundcheckradius = 0.5f;
    Quaternion lookRotation;
    float targetAngle;

    private BoxCollider _opc;

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
        controller = GetComponent<CharacterController>();
        Cursor.visible = false;
        _mainCamera = Camera.main;
        _sound = GetComponent<AudioSource>();
        time_jump = Time.time;
        _opc = GetComponentInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Try and align player's rotation with the angle of the ground. 
        //AlignWithGround();

        _movementSpeed = !_opc.enabled ? _movementSpeed : 0.18f;

        // check grounded if jump is not recent
        if (Time.time - time_jump > 0.1f) {
            bool temp = _isGrounded;
            _isGrounded = IsGrounded();
            if (_isGrounded && ! temp) {
                CreateDust();
            }
        }
        // Get the movement inputs.
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")).normalized;

        if (direction.magnitude >= 0.1f && _isGrounded)
        {
            // play walking sound
            if (_isGrounded && !_isPlayingWalkingSound) {
                 _isPlayingWalkingSound = true;
            }
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                _mainCamera.transform.eulerAngles.y;
            if (!_isThrowing)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(lookRotation.eulerAngles.x, targetAngle, lookRotation.eulerAngles.z),
                    Time.deltaTime * rotationSpeed);
            }
            direction = Quaternion.Euler(lookRotation.eulerAngles.x, targetAngle, lookRotation.eulerAngles.z) * Vector3.forward;
        } else if (direction.magnitude >= 0.1f) {
            // play walking sound
            if (_isGrounded && !_isPlayingWalkingSound) {
                 _isPlayingWalkingSound = true;
            }
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +
                                _mainCamera.transform.eulerAngles.y;
            direction = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        } else {
            _isPlayingWalkingSound = false;
        }

        if (_isGrounded) {
            if (transform.rotation.eulerAngles.x < 1f && transform.rotation.eulerAngles.z < 1f)
                velocity = Vector3.zero;
            AlignWithGround();
        }
        // JUMPING
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) {
            time_jump = Time.time;
            velocity.y = 0.11f;
            _isGrounded = false;
            _jumped = true;
        }
        // THROWING
        if (_isThrowing) {
            if (_firstThrow) {
                xAxis.Value = followTransform.rotation.eulerAngles.y;
                yAxis.Value = followTransform.rotation.eulerAngles.x;
                _firstThrow = false;
            }
            xAxis.Value += Input.GetAxis("Mouse X") * PlayerPrefs.GetFloat("sens") * 500f * Time.deltaTime;
            //yAxis.Value -= Input.GetAxis("Mouse Y") * PlayerPrefs.GetFloat("sens") * 1000 * Time.deltaTime;

            followTransform.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
            
            // Rotate the player to match the camera yaw
            float yawCamera = _mainCamera.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, yawCamera, 0f), rotationSpeed * Time.deltaTime);
        }
        if (_jumped && _isGrounded)
        {
            //_sound.clip = hitGround;
            _sound.loop = false;
            _sound.PlayOneShot(hitGround, 1.5f);
            _jumped = false;
            StartCoroutine(WaitForFallSoundToFinish());
        }
        //Animation publisher
        EventBus.Publish<MovementEvent>(new MovementEvent(_isPlayingWalkingSound, _jumped, _isGrounded));
    }

    void FixedUpdate() {
        controller.Move(direction.normalized * (_movementSpeed * baseMovementSpeed));
        // gravity
        if (!_isGrounded)
        {
            // jump mechanic
            if (Time.time - time_jump < jumpAppliedTime && velocity.y == 0.11f)
                velocity.y += 0.5f * jumpPower * Time.deltaTime * Time.deltaTime;
            _isPlayingWalkingSound = false;
            // lerp to zero rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetAngle, 0), Time.deltaTime * rotationSpeed);
        }
        velocity.y += 0.5f * gravity * Time.deltaTime * Time.deltaTime;
        controller.Move(velocity);
    }


    IEnumerator WaitForFallSoundToFinish()
    {
        yield return new WaitForSeconds(0.3f);
        _isPlayingWalkingSound = false;
    }

    private bool IsGrounded()
    {
        return (Physics.CheckBox(groundCheck.transform.position, new Vector3(groundcheckradius,groundDistance,groundcheckradius), Quaternion.identity, groundMask));
    }

    private void AlignWithGround()
    {
        // This function tries to align the player's rotation with the angle of the ground if they are on a slope.
        RaycastHit hitInfo;
        Debug.DrawRay(groundCheck.position, Vector3.down * 2f, Color.cyan);
        if (Physics.Raycast( groundCheck.position, Vector3.down, out hitInfo, 0.5f, groundMask))
        {
            // We hit the ground, calculate the angle of the player and our hit's normal.
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if (angle < 80f || angle > 280f)
            {
                Vector3 slopeForward = Vector3.Cross(transform.right, hitInfo.normal);
                lookRotation = Quaternion.LookRotation(slopeForward);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
                //_rb.freezeRotation = true;
            }
            else
            {
                //_rb.freezeRotation = false;
            }
        }
    }

    public void OnToggleThrowing()
    {
        _firstThrow = true;
        _isThrowing = !_isThrowing;
    }

    void CreateDust() {
        dust.Play();
    }
    void StopDust() {
        dust.Stop();
    }
}