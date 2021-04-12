using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTntReactor : MonoBehaviour
{
    Subscription<HitObjectEvent> hitObjectSubscription;

    public int health = 4;

    private int max_health;

    public Text UI;

    // Start is called before the first frame update
    void Start()
    {
        hitObjectSubscription = EventBus.Subscribe<HitObjectEvent>(_OnHitObject);
        max_health = health;

        if (UI) {
            UI.text = "Stuffalo Steal:  " + health + "/" + max_health;
        }
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
        if (UI) {
            UI.text = "WANTED\nStuffalo Steal:  " + health + "/" + max_health;
        }
        if (health == 0) {
            EventBus.Publish<LevelClearEvent>(new LevelClearEvent());
            Destroy(gameObject);
        }
    }

}
