using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_LoadLevel : MonoBehaviour
{
    Text level_name;

    void Start() {
        level_name = GetComponent<Text>();
    }

    public void OnButtonPress(){
        SceneManager.LoadScene(level_name.text);
    }
}
