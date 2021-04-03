using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneUponLevelClear : MonoBehaviour
{
    public string s = "Hub";

    private Subscription<LevelClearEvent> levelClearSubscription;
    AudioSource sound;
    public AudioClip mailshere;
    public GameObject winScreen;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        levelClearSubscription = EventBus.Subscribe<LevelClearEvent>(LoadScene);
    }

    void LoadScene(LevelClearEvent _event)
    {
        //Debug.Log("mailshere");
        sound.clip = mailshere;
        sound.Play();
        winScreen.SetActive(true);
        StartCoroutine(WaitForMailsHere());
    }

    IEnumerator WaitForMailsHere()
    {
        
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(s);
    }
}
