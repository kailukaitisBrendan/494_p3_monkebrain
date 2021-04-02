using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    private Rigidbody _rb;
    public float movementSpeed = 6f;
    public float jumpPower = 5f;

    public LayerMask groundMask;

    public Transform groundCheck;

    public float groundDistance = 0.4f;

    public float angleDamping = 0.1f;
    
    public AudioClip walkingSound;
    public AudioClip hitGround;
    
    private float _angleVelocity;
    private bool _isGrounded = false;
    private bool _isThrowing = false;
    private Camera _mainCamera;
    private bool _isPlayingWalkingSound = false;
    private AudioSource _sound;
    
    private bool _jumped = false;

    // Denotes base velocity (before factoring in player inputs)
    public Vector3 baseVelocity;

    private void Awake()
    {
        // Lock the cursor to the game window
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Start is called before the first frame update
    void Start()
    {
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
        Vector3 velocity = Vector3.zero;
        _isGrounded = IsGrounded();
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")).normalized;

        if (_isThrowing)
        {
            // If we are in the process of throwing an object, then our mouse x input should control the players 
            // rotation
            Vector3 angle = transform.eulerAngles;
            angle.y = _mainCamera.transform.eulerAngles.y;
            transform.eulerAngles = angle;
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
            //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _angleVelocity, angleDamping);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            velocity = moveDir.normalized * movementSpeed;
            //Debug.Log(velocity);
        }
        else
        {
            //stop walking sound
            // if (_sound.clip != hitGround)
            //     _sound.Stop();
            _isPlayingWalkingSound = false;


            velocity = Vector3.zero;
        }


        velocity.y = _rb.velocity.y;

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _isGrounded = false;
            velocity.y = jumpPower;
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