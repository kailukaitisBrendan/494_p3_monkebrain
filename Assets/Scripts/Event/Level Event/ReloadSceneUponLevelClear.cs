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
        if (winScreen) winScreen.SetActive(true);
        StartCoroutine(WaitForMailsHere());
        Debug.Log("Level cleared");
    }

    IEnumerator WaitForMailsHere()
    {
        
        yield return new WaitForSeconds(2);
        SceneTransitioner st = FindObjectOfType<SceneTransitioner>();
        if (st) st.LoadScene(s);
        else SceneManager.LoadScene(s);
    }
}
