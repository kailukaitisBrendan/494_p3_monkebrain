using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneUponLevelClear : MonoBehaviour
{
    private Subscription<LevelClearEvent> levelClearSubscription;

    void Start()
    {
        levelClearSubscription = EventBus.Subscribe<LevelClearEvent>(ReloadScene);
    }

    void ReloadScene(LevelClearEvent _event)
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

}
