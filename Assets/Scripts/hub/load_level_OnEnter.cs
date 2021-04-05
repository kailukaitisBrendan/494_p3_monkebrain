using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_level_OnEnter : MonoBehaviour
{
    public string s = "Main_Menu";
    GameObject g;
    GameObject c;

    void Start()
    {
        c = GameObject.FindGameObjectWithTag("MainCamera");
        g = GameObject.FindGameObjectWithTag("Wagon");
    }

    public void Update(){
        if (g == null || c == null) {
            c = GameObject.FindGameObjectWithTag("MainCamera");
            g = GameObject.FindGameObjectWithTag("Wagon");
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
            g.SetActive(false);
            c.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        
        SceneTransitioner st = FindObjectOfType<SceneTransitioner>();
        if (st) st.LoadScene(s);
        else SceneManager.LoadScene(s);
        }
    }
}
