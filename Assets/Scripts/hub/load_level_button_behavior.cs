using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class load_level_button_behavior : MonoBehaviour
{
    public string s = "Main_Menu";

    public void OnButtonPress(){
        Debug.Log("Transition!");
        Time.timeScale = 1f;
        if (s == "Main_Menu" || s == "Hub" || s == "Intro") {
            Cursor.visible = true;
        }
        
        SceneTransitioner st = FindObjectOfType<SceneTransitioner>();
        if (st) st.LoadScene(s);
        else SceneManager.LoadScene(s);
    }
}
