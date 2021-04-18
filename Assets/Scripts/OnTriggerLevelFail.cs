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
            if (sound) {
                sound.Play();
            }

            // Disable player object interaction
            GameObject player = other.gameObject;
            if (player.GetComponent<ObjectInteraction>()) {
                player.GetComponent<ObjectInteraction>().enabled = false;
            }

            // Disable Camera
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("CineMachineController"))
            {
                obj.SetActive(false);
            }

            StartCoroutine(WaitForScream());
        }
    }

    private void Update()
    {
        if (fallen)
        {
            fallen = false;
            bool wasFall = true;
            EventBus.Publish<LevelFailEvent>(new LevelFailEvent(wasFall));
        }

    }

    IEnumerator WaitForScream()
    {
        
        yield return new WaitForSeconds(1.5f);
        fallen = true;

    }
}

