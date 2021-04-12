using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tumbleweedRotator : MonoBehaviour
{
    float rotat;
    float speed = 500f;
    void Start() {
        rotat = 0.0f;
    }
    void FixedUpdate() {
        rotat += speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(-rotat * Mathf.Sin(transform.rotation.y), 0, -rotat * Mathf.Cos(transform.rotation.y));
    }
}
