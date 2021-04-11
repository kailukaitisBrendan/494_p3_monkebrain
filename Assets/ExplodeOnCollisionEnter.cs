using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnCollisionEnter : MonoBehaviour
{
    Subscription<HitObjectEvent> hitObjectSubscription;
    public ParticleSystem dust;
    public GameObject mesh;
    AudioSource thump;
    private bool hasBeenPlaced = false;
    private void Awake() {
        thump = GetComponent<AudioSource>();
        hitObjectSubscription = EventBus.Subscribe<HitObjectEvent>(_OnHitObject);
    }

    void _OnHitObject(HitObjectEvent e) {
        if (e.sourceObject != gameObject) return;
        mesh.SetActive(false);
        hasBeenPlaced = true;
        dust.Play();
    }

    private void Update()
    {
        if (hasBeenPlaced && !dust.IsAlive()) {
            Destroy(gameObject);
        }
    }

    void CreateDust() {
        mesh.SetActive(false);
        dust.Play();
    }
    void StopDust() {
        dust.Stop();
        Destroy(gameObject);
    }
}
