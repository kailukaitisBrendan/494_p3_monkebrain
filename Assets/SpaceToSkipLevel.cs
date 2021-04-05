using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpaceToSkipLevel : MonoBehaviour
{
    public UnityEvent onLevelSkip;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
        {
            onLevelSkip.Invoke();
        }
    }
}
