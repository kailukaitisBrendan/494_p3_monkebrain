using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPromptManager : MonoBehaviour
{
    private GameObject textPrompt;
    private ObjectInteraction objInter;
    
    private bool jumped = false;

    // Start is called before the first frame update
    void Start()
    {
        textPrompt = GameObject.FindGameObjectWithTag("TextPrompt");
        objInter = gameObject.GetComponent<ObjectInteraction>();
    }
 
    // Update is called once per frame
    void Update()
    {
        if (!textPrompt) return;

        if (!jumped) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                jumped = true;
                textPrompt.SetActive(false);
            }
        }
        else if (objInter) {
            if (objInter.GetItem()) {
                textPrompt.SetActive(true);
                textPrompt.GetComponent<TextMesh>().text = "E";
            }
            else {
                textPrompt.SetActive(false);
            }
        }
    }

    // Check for 
}
