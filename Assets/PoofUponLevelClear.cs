using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoofUponLevelClear : MonoBehaviour
{
    Subscription<LevelClearEvent> levelClearSubscription;
    public AudioClip poof_sound;
    private AudioSource audio;
    public ParticleSystem poof;

    private void Awake() {
        audio = GetComponent<AudioSource>();
        levelClearSubscription = EventBus.Subscribe<LevelClearEvent>(_OnLevelClear);
    }

    void _OnLevelClear(LevelClearEvent e) {
        // Disable MeshRenderers
        MeshRenderer mr = null;
        foreach (Transform child in transform) {
            if (mr = child.GetComponent<MeshRenderer>()) {
                mr.enabled = false;
            }
        }

        // Disable Colliders
        GetComponent<BoxCollider>().enabled = false;

        if (poof_sound) {
            audio.loop = false;
            audio.PlayOneShot(poof_sound, 5f);
        }
        poof.Play();
    }

}
