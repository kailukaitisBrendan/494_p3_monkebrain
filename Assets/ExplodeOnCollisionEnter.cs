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
        if (hasBeenPlaced) return;
        
        // Hide mesh
        mesh.SetActive(false);

        // Disable Collider
        GetComponent<CapsuleCollider>().isTrigger = true;
        
        // Cancel movement
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        hasBeenPlaced = true;
        thump.Play();
        dust.Play();
    }

    private void Update()
    {
        if (hasBeenPlaced && !dust.IsAlive()) {
            Respawnable resp = GetComponent<Respawnable>();

            if (resp && resp.enabled) {
                mesh.SetActive(true);
                GetComponent<CapsuleCollider>().isTrigger = false;
                resp.Respawn();
                hasBeenPlaced = false;
            }
            else {
                Destroy(gameObject);
            }
        }
    }

}
