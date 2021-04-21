using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneUponLevelFail : MonoBehaviour
{
    private Subscription<LevelClearEvent> levelClearSubscription;
    private Subscription<LevelFailEvent> levelFailSubscription;
    AudioSource sound;
    public AudioClip stickemup;
    public GameObject loseScreen;
    private bool levelCleared = false;
    
    void Start()
    {
        sound = GetComponent<AudioSource>();
        levelFailSubscription = EventBus.Subscribe<LevelFailEvent>(ReloadScene);
        levelClearSubscription = EventBus.Subscribe<LevelClearEvent>(OnLevelClear);
    }

    void OnLevelClear(LevelClearEvent _event)
    {
        levelCleared = true;
    }

    void ReloadScene(LevelFailEvent _event)
    {
        if (!_event.wasFall)
        {
            sound.clip = stickemup;
            sound.Play();
        }
        //Debug.Log("sound?");
        if (loseScreen) loseScreen.SetActive(true);
        StartCoroutine(WaitThenLoad());
        Debug.Log("Level failed, was fall? " + _event.wasFall);
    }


    IEnumerator WaitThenLoad()
    {
        yield return new WaitForSeconds(1.5f);

        if (levelCleared) yield break;
        string sceneIndex = SceneManager.GetActiveScene().name;
        SceneTransitioner st = FindObjectOfType<SceneTransitioner>();
        if (st) st.LoadScene(sceneIndex);
        else SceneManager.LoadScene(sceneIndex);
    }
}
