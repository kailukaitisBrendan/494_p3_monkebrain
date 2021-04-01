using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thumpOnCollisionEnter : MonoBehaviour
{
    AudioSource thump;
    private bool hasBeenPlaced = false;
    private void Awake()
    {
        thump = GetComponent<AudioSource>();
        StartCoroutine(WaitForPlacement());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12)
        {
            if (hasBeenPlaced)
                thump.Play();
        }
    }

    IEnumerator WaitForPlacement()
    {
        yield return new WaitForSeconds(1);
        hasBeenPlaced = true;
    }



}
