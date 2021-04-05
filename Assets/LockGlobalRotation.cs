﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockGlobalRotation : MonoBehaviour
{
    private Quaternion initRotation;

    // Start is called before the first frame update
    void Start()
    {
        initRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = initRotation;
    }
}
