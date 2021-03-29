using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneUponLevelFail : MonoBehaviour
{
    private Subscription<LevelFailEvent> levelClearSubscription;
    AudioSource sound;
    public AudioClip stickemup;
    void Start()
    {
        sound = GetComponent<AudioSource>();
        levelClearSubscription = EventBus.Subscribe<LevelFailEvent>(ReloadScene);
    }

    void ReloadScene(LevelFailEvent _event)
    {
        sound.clip = stickemup;
        sound.Play();
        //Debug.Log("sound?");
        StartCoroutine(WaitThenLoad());
        
    }


    IEnumerator WaitThenLoad()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
