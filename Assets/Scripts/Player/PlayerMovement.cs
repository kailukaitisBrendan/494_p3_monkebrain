using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float movementSpeed = 6f;
    public float jumpPower = 5f;
    public float climbSpeed = 3f;


    public LayerMask Climbable;
    
    // Denotes the maximum angle for a surface below to be considered as 'ground' (vs wall)
    private float maxGroundAngle = Mathf.PI / 6;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    Vector3 GetInput()
    {
        Vector3 direction = Vector3.zero;
        float count = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            direction += cam.transform.forward;
            count++;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            direction += -cam.transform.forward;
            count++;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction += cam.transform.right;
            count++;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction += -cam.transform.right;
            count++;
        }

        direction.y = 0;
        direction = direction.normalized;

        return direction;
    }

    void Rotate(Vector3 force)
    {
        if (IsGrounded() && !Input.GetKey(KeyCode.Space) && force.y == 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(force), 0.1f);
        }
        else
        {
            Vector3 newRot = force;
            newRot.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newRot), 0.1f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newForce = GetInput() * movementSpeed;

        if (newForce != Vector3.zero)
        {
            Rotate(newForce);
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        bool isJumpingOrClimbing = false;

        //CLIMBING
        if (newForce != Vector3.zero && ExistsForwardWall())
        {
            newForce = transform.forward;
            newForce.y = climbSpeed;
            //Debug.Log("climb");
            isJumpingOrClimbing = true;
        }
        
        //ANTI WALL STICK 
        if (newForce != Vector3.zero && ExistsForwardWallNotClimbable())
        {
            newForce = rb.velocity;
        }

        //JUMP 
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            newForce.y = jumpPower;
            //Debug.Log("jump");
            isJumpingOrClimbing = true;
        }

        if (!isJumpingOrClimbing)
        {
            newForce.y = rb.velocity.y;
        }

        rb.velocity = newForce;
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        float dist = 1.1f;
        Vector3 offset = new Vector3(0, 0, 0);
        Debug.DrawRay(transform.position + offset, Vector3.down, Color.cyan);
        if (Physics.Raycast(transform.position + offset, Vector3.down, out hit, dist))
        {
            Vector3 surfaceNormal = hit.normal.normalized;

            if (surfaceNormal.z < 0) surfaceNormal *= -1f;

            float sinMax;
            float sin;
            sin = surfaceNormal.z;
            sinMax = Mathf.Sin(maxGroundAngle);

            // Also rotate with ground any applicable children
            foreach (Transform child in transform) {
                if (child.GetComponent<RotateWithGround>()) {
                    child.GetComponent<RotateWithGround>().Align(transform.rotation * surfaceNormal);
                }
            }

            if (sin <= sinMax) return true;
            else return false;
        }

        return false;
    }


    public bool ExistsForwardWall()
    {
        RaycastHit hit;
        float dist = 0.7f;

        Vector3 toes = new Vector3(0, -1f, 0);
        Vector3 knees = new Vector3(0, -0.5f, 0);
        Vector3 head = new Vector3(0, 1, 0);


        Debug.DrawRay(transform.position + toes, transform.forward, Color.cyan);
        Debug.DrawRay(transform.position + knees, transform.forward, Color.cyan);
        Debug.DrawRay(transform.position + head, transform.forward, Color.cyan);


        if (Physics.Raycast(transform.position + toes, transform.forward, out hit, dist, Climbable)
            || Physics.Raycast(transform.position + knees, transform.forward, out hit, dist, Climbable)
            || Physics.Raycast(transform.position + head, transform.forward, out hit, dist, Climbable)
        )
        {
            // Ignore if we are currently holding the climbable object.
            return hit.transform.parent == null;
        }

        return false;
    }


    public bool ExistsForwardWallNotClimbable()
    {
        RaycastHit hit;
        float dist = 0.7f;

        Vector3 left = new Vector3(-0.5f, 0, 0);
        Vector3 mid = new Vector3(0.5f, 0, 0);
        Vector3 right = new Vector3(0, 1, 0);
        Vector3 foot = new Vector3(0, -1, 0);


        Debug.DrawRay(transform.position + left, transform.forward, Color.blue);
        Debug.DrawRay(transform.position + mid, transform.forward, Color.blue);
        Debug.DrawRay(transform.position + right, transform.forward, Color.blue);
        Debug.DrawRay(transform.position + foot, transform.forward, Color.blue);



        if (Physics.Raycast(transform.position + left, transform.forward, out hit, dist, ~Climbable)
            || Physics.Raycast(transform.position + mid, transform.forward, out hit, dist, ~Climbable)
            || Physics.Raycast(transform.position + right, transform.forward, out hit, dist, ~Climbable)
            || Physics.Raycast(transform.position + foot, transform.forward, out hit, dist, ~Climbable)
        )
        {
            // Ignore if we are currently holding the NOT climbable object.
            Debug.Log("BANG0");
            return hit.transform.parent == null;
        }

        return false;
    }

    
}