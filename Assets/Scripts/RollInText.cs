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

    private void Awake()
    {
        textComponent = this.GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator ShowText()
    {
        for (int i = 0; i < fullMessage.Length; ++i)
        {
            currentMessage += fullMessage[i];
            textComponent.text = currentMessage;
            yield return new WaitForSeconds(letterDelay);
        }
        currentMessage = "";
    }
}
