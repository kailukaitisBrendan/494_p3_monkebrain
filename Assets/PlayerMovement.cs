using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    float movementSpeed = 5f;
    public LayerMask Climbable;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newVelocity = rb.velocity;

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 xzmove = cam.transform.forward;
            xzmove.y = 0;
            newVelocity = xzmove * movementSpeed;

            //Rotation
            if (IsGrounded() && !Input.GetKey(KeyCode.Space) && newVelocity.y == 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newVelocity), 0.1f);
            }
            else
            {
                Vector3 newRot = newVelocity;
                newRot.y = 0;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newRot), 0.1f);
            }

        }
        

        if (Input.GetKey(KeyCode.S))
        {
            Vector3 xzmove = cam.transform.forward;
            xzmove.y = 0;
            newVelocity = xzmove * -movementSpeed;

            //Rotation
            if (IsGrounded() && !Input.GetKey(KeyCode.Space) && newVelocity.y == 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newVelocity), 0.1f);
            }
            else
            {
                Vector3 newRot = newVelocity;
                newRot.y = 0;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newRot), 0.1f);
            }
        }
        

        if (Input.GetKey(KeyCode.A))
        {
            Vector3 xzmove = cam.transform.forward;
            xzmove.y = 0;
            Vector3 left = Vector3.Cross(xzmove, Vector3.up).normalized;
            newVelocity = left * movementSpeed;

            //Rotation
            if (IsGrounded() && !Input.GetKey(KeyCode.Space) && newVelocity.y == 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newVelocity), 0.1f);
            }
            else
            {
                Vector3 newRot = newVelocity;
                newRot.y = 0;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newRot), 0.1f);
            }

        }
        
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 xzmove = cam.transform.forward;
            xzmove.y = 0;
            Vector3 left = Vector3.Cross(xzmove, Vector3.up).normalized;
            newVelocity = left * -movementSpeed;

            //Rotation
            if (IsGrounded() && !Input.GetKey(KeyCode.Space) && newVelocity.y == 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newVelocity), 0.1f);
            }
            else
            {
                Vector3 newRot = newVelocity;
                newRot.y = 0;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newRot), 0.1f);
            }

        }
        
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            newVelocity.x = 0;
            newVelocity.z = 0;
        }


        //CLIMBING 


        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && ExistsForwardWall())
        {

            newVelocity = transform.forward;
            newVelocity.y = 3;
            //Debug.Log("Hi");
        }
        


        rb.velocity = newVelocity;
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        float dist = 1.01f;
        Vector3 offset = new Vector3(0, 1, 0);
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
        float dist = 0.7f;
        
        Vector3 toes = new Vector3(0, -1f, 0);
        Vector3 knees = new Vector3(0, -0.5f, 0);
        Vector3 head = new Vector3(0, 1, 0);
       


        Debug.DrawRay(transform.position + toes, Vector3.forward, Color.cyan);
        Debug.DrawRay(transform.position + knees, Vector3.forward, Color.cyan);
        Debug.DrawRay(transform.position + head, Vector3.forward, Color.cyan);
        

        if (Physics.Raycast(transform.position + toes, transform.forward, out hit, dist, Climbable)
            || Physics.Raycast(transform.position + knees, transform.forward, out hit, dist, Climbable)
            || Physics.Raycast(transform.position + head, transform.forward, out hit, dist, Climbable)
            )
        {

            return true;

        }

        return false;
    }


 


}
