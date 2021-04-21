using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingCredits : MonoBehaviour
{
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Time.deltaTime * new Vector3(0f, speed, 0f);
    }
}
