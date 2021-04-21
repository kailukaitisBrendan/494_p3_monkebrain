using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnLevelClear : MonoBehaviour
{
    Subscription<LevelClearEvent> levelClearSubscription;


    // Start is called before the first frame update
    void Start()
    {
        levelClearSubscription = EventBus.Subscribe<LevelClearEvent>(OnLevelClear);
    }

    void OnLevelClear(LevelClearEvent _event) {
        gameObject.SetActive(false);
    }
}
