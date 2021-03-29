using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerLevelFail : MonoBehaviour {
    private AudioSource sound;
    private bool fallen = false;


    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sound.Play();
            StartCoroutine(WaitForScream());
        }
    }

    private void Update()
    {
        if (fallen)
        {
            fallen = false;
            EventBus.Publish<LevelFailEvent>(new LevelFailEvent());
        }
    }

    IEnumerator WaitForScream()
    {
        
        yield return new WaitForSeconds(1.5f);
        fallen = true;
    }
}
