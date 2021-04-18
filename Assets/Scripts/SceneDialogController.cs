using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SceneDialogController : MonoBehaviour
{
    public string[] messages;
    //RollInText rollInText;
    //public float textDuration = 3f;
    public float letterDelay = .1f;
    int _messagesIndex = 0;
    private bool _isTyping = false;
    private bool _cancelTyping = false;
    
    private TextMeshProUGUI textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    public void PlayNextMessage()
    {
        if (_isTyping)
        {
            _cancelTyping = true;
        }
        StopAllCoroutines();
        textComponent.text = messages[_messagesIndex];
        StartCoroutine(TextScroll(messages[_messagesIndex]));
        ++_messagesIndex;
        
        
        // switch (_isTyping)
        // {
        //     case false:
        //         StartCoroutine(TextScroll(messages[_messagesIndex]));
        //         ++_messagesIndex;
        //         break;
        //     case true when !_cancelTyping:
        //         _cancelTyping = true;
        //         break;
        // }

        Debug.Log("Playing Message");
        // rollInText.fullMessage = messages[messagesIndex];
        // rollInText.cancelTyping = true;
        //rollInText.StopAllCoroutines();
    }

    private IEnumerator TextScroll(string text)
    {
        int letter = 0;
        textComponent.text = "";
        _cancelTyping = false;
        _isTyping = true;

        while (_isTyping && !_cancelTyping && (letter < text.Length  - 1))
        { 
            textComponent.text += text[letter];
            letter++;
            yield return new WaitForSeconds(letterDelay);
        }

        textComponent.text = text;
        _isTyping = false;
        _cancelTyping = false;
    }

}
