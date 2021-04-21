using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTntReactor : MonoBehaviour
{
    Subscription<HitObjectEvent> hitObjectSubscription;

    public int health = 5;

    private int max_health;

    public Image UI;

    private float max_offset_max_x;
    private float offset_min_x;

    AudioSource sound;
    public AudioClip hitObjectSound;
    public AudioClip squish;
    public ParticleSystem hitAnimation;


    // Start is called before the first frame update
    void Start()
    {
        hitObjectSubscription = EventBus.Subscribe<HitObjectEvent>(_OnHitObject);
        max_health = health;
        sound = GetComponent<AudioSource>();
    }

    void _OnHitObject(HitObjectEvent e) {
        if (e.hitObject != gameObject) return;
        if (e.sourceObject.tag != "TNT") return;
        
        // Wake up if applicable
        GetComponent<ThrowPeriodically>().enabled = true;
        GetComponent<FacePlayer>().enabled = true;

        // Disable source object respawnability if applicable
        if (e.sourceObject.GetComponent<Respawnable>())
            e.sourceObject.GetComponent<Respawnable>().enabled = false;

        // Increase Throw Rate
        if (GetComponent<ThrowPeriodically>()) {
            GetComponent<ThrowPeriodically>().OnDamage();
        }

        --health;
        sound.clip = hitObjectSound;
        sound.Play();
        sound.clip = squish;
        sound.Play();
        hitAnimation.Play();

        if (UI) {
            UI.fillAmount = (float)(health) / (float)(max_health);
        }
        if (health == 0) {
            EventBus.Publish<LevelClearEvent>(new LevelClearEvent());
            Globals.gameComplete = true;
        }
    }

}
