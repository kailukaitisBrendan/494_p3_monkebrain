using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneUponLevelFail : MonoBehaviour
{
    private Subscription<LevelFailEvent> levelFailSubscription;
    AudioSource sound;
    public AudioClip stickemup;
    public GameObject loseScreen;
    void Start()
    {
        sound = GetComponent<AudioSource>();
        levelFailSubscription = EventBus.Subscribe<LevelFailEvent>(ReloadScene);
    }

    void ReloadScene(LevelFailEvent _event)
    {
        sound.clip = stickemup;
        sound.Play();
        //Debug.Log("sound?");
        loseScreen.SetActive(true);
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
