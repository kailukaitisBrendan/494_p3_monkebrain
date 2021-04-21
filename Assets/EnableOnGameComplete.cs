using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnGameComplete : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Globals.gameComplete) {
            foreach (Transform child in transform) {
                child.gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
