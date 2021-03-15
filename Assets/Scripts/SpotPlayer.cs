using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPlayer : MonoBehaviour
{
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
        if (other.gameObject.CompareTag("Player"))
        {
            EventBus.Publish<PlayerSpottedEvent>(new PlayerSpottedEvent(other.gameObject, transform.parent.gameObject));
        }
    }
}
