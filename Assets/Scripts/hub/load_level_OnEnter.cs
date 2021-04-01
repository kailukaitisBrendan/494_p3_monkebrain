using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_level_OnEnter : MonoBehaviour
{
    public string s = "Main_Menu";

    public void Update(){
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(s);
    }
}
