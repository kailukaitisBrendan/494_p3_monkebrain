using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class able_object_button_behavior : MonoBehaviour
{
    public GameObject s;
    public bool b;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnButtonPress();
        }
    }

    public void OnButtonPress(){
        //print("buttonpress");
        s.SetActive(b);
        Cursor.visible = b;
        if (!b) {
            Time.timeScale = 1f;
        } else {
            Time.timeScale = 0f;
        }
    }
}