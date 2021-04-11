using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTntReactor : MonoBehaviour
{
    Subscription<HitObjectEvent> hitObjectSubscription;

    public int health = 4;

    // Start is called before the first frame update
    void Start()
    {
        hitObjectSubscription = EventBus.Subscribe<HitObjectEvent>(_OnHitObject);
    }

    void _OnHitObject(HitObjectEvent e) {
        if (e.hitObject != gameObject) return;
        
        // Wake up if applicable
        GetComponent<ThrowPeriodically>().enabled = true;
        GetComponent<FacePlayer>().enabled = true;

        // Disable source object respawnability if applicable
        if (e.sourceObject.GetComponent<Respawnable>())
            e.sourceObject.GetComponent<Respawnable>().enabled = false;

        --health;
        if (health == 0) {
            EventBus.Publish<LevelClearEvent>(new LevelClearEvent());
            Destroy(gameObject);
        }
    }

}
