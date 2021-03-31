using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float movementSpeed = 6f;
    public float jumpPower = 5f;
    public float climbSpeed = 3f;

    public LayerMask groundLayer;
    public LayerMask Climbable;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;


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



    RaycastHit hit;
    void Slide()
    {

        
        Vector3 checkpos = transform.position - transform.up;
        //checkpos.x += transform.forward.x;
        //checkpos.z += transform.forward.z;
        float dist = 1f;
        Debug.DrawRay(checkpos, Vector3.down * dist, Color.cyan);


        
        if (Physics.Raycast(checkpos, Vector3.down, out hit, dist, groundLayer))
        {

            if (Vector3.Angle(hit.normal, Vector3.up) > 30)
            {
                rb.freezeRotation = false;

            }
            else
            {

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal)), 10 * Time.deltaTime);
                rb.freezeRotation = true;
            }
        }


    }

    bool jumped;

    // Update is called once per frame
    void FixedUpdate()
    {
        


        Vector3 newForce = GetInput() * movementSpeed;

        Slide();



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
        //Debug.Log(newForce);
        rb.velocity = newForce;

    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        float dist = 1.01f;
        Vector3 offset = new Vector3(0, 0, 0);
        Debug.DrawRay(transform.position + offset, Vector3.down, Color.cyan);
        if (Physics.Raycast(transform.position + offset, Vector3.down, out hit, dist))
        {
            return true;
        }

        return false;
    }



    public bool ExistsForwardWall()
    {
        RaycastHit hit;
        float dist = 1f;

        Vector3 toes = new Vector3(0, 0f, 0);
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

    





}