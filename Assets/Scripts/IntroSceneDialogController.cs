using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntroSceneDialogController : MonoBehaviour
{
    public string[] messages;
    RollInText rollInText;
    public float textDuration = 3f;
    int messagesIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        rollInText = GetComponent<RollInText>();
    }

    // public void OnCameraLive()
    // {
    //     Debug.Log("Hello!");
    // }

    IEnumerator StartOldManSpeech()
    {
        //gameObject.SetActive(true);
        for (int i = 0; i < messages.Length; ++i)
        {
            Debug.Log("Here");
            rollInText.fullMessage = messages[i];
            yield return StartCoroutine(rollInText.ShowText());
            yield return new WaitForSeconds(textDuration);
        }
        
        // Text is done.
        //gameObject.SetActive(false);
    }

    public void PlayNextMessage()
    {
        Debug.Log("Playing Message");
        rollInText.fullMessage = messages[messagesIndex];
        StartCoroutine(rollInText.ShowText());
        ++messagesIndex;
    }

}
