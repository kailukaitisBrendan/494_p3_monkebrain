using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wagon_behavior : MonoBehaviour
{
    public ParticleSystem dust;

    Rigidbody rb;
    public Camera cam;
    public float movementSpeed = 6f;
    public static wagon_behavior instance = null;  
    void Start()
    {
        //Check if instance already exists
        if (instance == null) {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this) {
            instance.gameObject.SetActive(true);
            Destroy(gameObject);
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
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


        if (!Input.GetKey(KeyCode.Space) && force.y == 0)
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
        if (Mathf.Abs(newForce.x) > 0.01f 
            || Mathf.Abs(newForce.y) > 0.01f 
            || Mathf.Abs(newForce.z) > 0.01f)
            CreateDust();
        else {
            StopDust();
        }
        rb.velocity = newForce;

        if (newForce != Vector3.zero)
        {
            Rotate(newForce);
        }
        else
        {
            rb.velocity = new Vector3(0f, 0f, 0f);

        }
    }

    void CreateDust() {
        dust.Play();
    }
    void StopDust() {
        dust.Stop();
    }
}
