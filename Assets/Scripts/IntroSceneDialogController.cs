using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSceneDialogController : MonoBehaviour
{
    public string[] messages;
    RollInText rollInText;
    // Start is called before the first frame update
    void Start()
    {
        rollInText = GetComponent<RollInText>();
        StartCoroutine(StartOldManSpeech());
    }

    IEnumerator StartOldManSpeech()
    {
        for (int i = 0; i < messages.Length; ++i)
        {
            rollInText.fullMessage = messages[i];
            yield return StartCoroutine(rollInText.ShowText());
            yield return new WaitForSeconds(3f);
        }
    }

}
