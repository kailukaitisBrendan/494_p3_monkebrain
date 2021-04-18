using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButtonEventUponCollision : MonoBehaviour
{
    Subscription<HitObjectEvent> hitObjectSubscription;
    private float blastRadius = 2f;

    // Start is called before the first frame update
    void Start()
    {
        hitObjectSubscription = EventBus.Subscribe<HitObjectEvent>(_OnHitObject);
    }

    void _OnHitObject(HitObjectEvent e) {
        Vector3 displacement = e.sourceObject.transform.position - transform.position;

        if (displacement.sqrMagnitude > blastRadius * blastRadius) return;
        Debug.Log(displacement.magnitude);
        GetComponent<TriggerButtonEventUponPress>().Press();
    }

}
