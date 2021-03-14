using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerLevelFail : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EventBus.Publish<LevelFailEvent>(new LevelFailEvent());
        }
    }
}
