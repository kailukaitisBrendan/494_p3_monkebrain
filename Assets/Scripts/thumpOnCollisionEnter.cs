using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thumpOnCollisionEnter : MonoBehaviour
{
    Subscription<HitObjectEvent> hitObjectSubscription;
    public ParticleSystem dust;
    AudioSource thump;
    private bool hasBeenPlaced = false;
    private void Awake() {
        thump = GetComponent<AudioSource>();
        hitObjectSubscription = EventBus.Subscribe<HitObjectEvent>(_OnHitObject);
    }

    void _OnHitObject(HitObjectEvent e) {
        if (e.justRespawned) return;
        if (e.sourceObject != gameObject) return;
        thump.Play();
        dust.Play();
    }

    void CreateDust() {
        dust.Play();
    }
    void StopDust() {
        dust.Stop();
    }

}
