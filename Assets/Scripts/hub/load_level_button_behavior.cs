using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class load_level_button_behavior : MonoBehaviour
{
    public string s = "Main_Menu";

    public void OnButtonPress(){
        Time.timeScale = 1f;
        if (s == "Main_Menu" || s == "Hub")
            Cursor.lockState = CursorLockMode.None;
        
        SceneTransitioner st = FindObjectOfType<SceneTransitioner>();
        if (st) st.LoadScene(s);
        else SceneManager.LoadScene(s);
    }
}
