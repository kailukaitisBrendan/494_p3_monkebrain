using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RollInText : MonoBehaviour
{
    TextMeshProUGUI textComponent;
    public string fullMessage;
    string currentMessage = "";
    public float letterDelay = .1f;
    public bool cancelTyping = false;

    private void Awake()
    {
        textComponent = this.GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator ShowText()
    {
        cancelTyping = false;
        for (int i = 0; i < fullMessage.Length && !cancelTyping; ++i)
        {
            currentMessage += fullMessage[i];
            textComponent.text = currentMessage;
            yield return new WaitForSeconds(letterDelay);
        }
        currentMessage = "";
        textComponent.text = fullMessage;
    }
    
}
