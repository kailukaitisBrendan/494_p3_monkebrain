using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPlayer : MonoBehaviour
{
    bool hasBeenSpotted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasBeenSpotted)
        {
            hasBeenSpotted = true;
            EventBus.Publish<PlayerSpottedEvent>(new PlayerSpottedEvent(other.gameObject, transform.parent.gameObject));
        }
    }
}
