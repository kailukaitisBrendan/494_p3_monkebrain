﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tumbleweedMovement : MonoBehaviour
{
    GameObject cam;
    float time_spawn;
    float time_run = 6.0f;
    float speed = 8f;
    float rot;
    void Start() {
        // generate random movement vector
        rot = Random.Range(-180f,180f);
        transform.rotation = Quaternion.Euler(0, rot, 0);
        time_spawn = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate() {
        // move in random direction
        transform.position += new Vector3(speed * Time.deltaTime * Mathf.Sin(rot),0f,speed * Time.deltaTime* Mathf.Cos(rot));
        if (cam != null) {
            if (Mathf.Abs(gameObject.transform.position.x - cam.transform.position.x) > 200f 
            || Mathf.Abs(gameObject.transform.position.z - cam.transform.position.z) > 200f) {
                Destroy(gameObject);
            }
        }
        if (Time.time - time_spawn > time_run) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.layer != 12) {
            Destroy(gameObject);
        }
    }

    public void SetCam(GameObject came) {
        cam = came;
    }
}
