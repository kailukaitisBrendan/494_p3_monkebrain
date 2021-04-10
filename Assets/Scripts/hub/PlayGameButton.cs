using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGameButton : MonoBehaviour
{
    public string s = "Intro";
    public string s1 = "Hub";

    void Awake() {
        if (Time.time < 0.1f) {
            PlayerPrefs.SetInt("PlayedIntro", 0);
            PlayerPrefs.SetFloat("sens", 0.3f);
            PlayerPrefs.SetFloat("volume", 1.0f);
        }
    }
    public void OnButtonPress() {
        Debug.Log("Transition!");
        Time.timeScale = 1f;
        if (s1 == "Main_Menu" || s1 == "Intro" || s1 == "Hub") {
            Cursor.visible = true;
        }
        if (PlayerPrefs.GetInt("PlayedIntro") == 1) {
            SceneTransitioner st = FindObjectOfType<SceneTransitioner>();
                if (st) st.LoadScene(s1);
                else SceneManager.LoadScene(s1);
        }
        else {
            PlayerPrefs.SetInt("PlayedIntro", 1);
            SceneTransitioner st = FindObjectOfType<SceneTransitioner>();
                if (st) st.LoadScene(s);
                else SceneManager.LoadScene(s);

        }
    }
}
