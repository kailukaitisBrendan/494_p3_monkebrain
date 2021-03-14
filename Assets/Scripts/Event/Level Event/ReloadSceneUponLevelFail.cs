using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneUponLevelFail : MonoBehaviour
{
    private Subscription<LevelFailEvent> levelClearSubscription;

    void Start()
    {
        levelClearSubscription = EventBus.Subscribe<LevelFailEvent>(ReloadScene);
    }

    void ReloadScene(LevelFailEvent _event)
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

}
