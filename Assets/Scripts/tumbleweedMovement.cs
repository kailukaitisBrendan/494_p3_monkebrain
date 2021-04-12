using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tumbleweedMovement : MonoBehaviour
{
    float speed = 8f;
    float rot;
    void Start() {
        // generate random movement vector
        rot = Random.Range(-180f,180f);
        transform.rotation = Quaternion.Euler(0, rot, 0);
    }

    // Update is called once per frame
    void FixedUpdate() {
        // move in random direction
        transform.position += new Vector3(speed * Time.deltaTime * Mathf.Sin(rot),0f,speed * Time.deltaTime* Mathf.Cos(rot));
    }
}
