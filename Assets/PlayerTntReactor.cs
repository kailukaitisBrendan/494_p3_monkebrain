using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTntReactor : MonoBehaviour
{
    Subscription<HitObjectEvent> hitObjectSubscription;

    private float blastRadius = 3f;

    // Start is called before the first frame update
    void Start()
    {
        hitObjectSubscription = EventBus.Subscribe<HitObjectEvent>(_OnHitObject);
    }

    void _OnHitObject(HitObjectEvent e) {
        if (e.sourceObject.tag != "TNT") return;
        if (e.justRespawned) return;

        Vector3 displacement = e.sourceObject.transform.position - transform.position;

        if (displacement.sqrMagnitude > blastRadius * blastRadius) return;

        Debug.Log("Player hit!");
        
        // Level Fail
        EventBus.Publish<LevelFailEvent>(new LevelFailEvent(true));
    }

}
