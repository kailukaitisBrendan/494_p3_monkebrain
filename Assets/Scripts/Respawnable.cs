using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawnable : MonoBehaviour
{
    private Vector3 initPosition;
    private Quaternion initRotation;
    private int respawnCounter = 0;

    public AudioClip poof_sound;
    private AudioSource audio;
    public ParticleSystem poof;

    // Start is called before the first frame update
    private void Start()
    {
        initPosition = transform.position;
        initRotation = transform.rotation;
        audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (respawnCounter > 0) --respawnCounter;
    }

    public bool JustRespawned()
    {
        return respawnCounter > 0;
    }

    public void Respawn()
    {
        EventBus.Publish<RespawnEvent>(new RespawnEvent(this.gameObject));

        transform.position = initPosition;
        transform.rotation = initRotation;

        respawnCounter = 30;
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb) {
            rb.velocity = new Vector3(0f,0f,0f); 
            rb.angularVelocity = new Vector3(0f,0f,0f);
        }

        poof.Play();
        if (poof_sound) {
            audio.loop = false;
            audio.PlayOneShot(poof_sound, 5f);
        }

    }

}
