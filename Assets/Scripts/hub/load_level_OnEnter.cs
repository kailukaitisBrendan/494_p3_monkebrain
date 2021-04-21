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
            // this isn't very elegant but it works
            if (s == "World0-0" && PlayerPrefs.GetInt("levelLock") < 1) {
                PlayerPrefs.SetInt("levelLock",1);
            } else if (s == "World0-1" && PlayerPrefs.GetInt("levelLock") < 2) {
                PlayerPrefs.SetInt("levelLock",2);
            } else if (s == "New1-0" && PlayerPrefs.GetInt("levelLock") < 3) {
                PlayerPrefs.SetInt("levelLock",3);
            } else if (s == "New1-1" && PlayerPrefs.GetInt("levelLock") < 4) {
                PlayerPrefs.SetInt("levelLock",4);
            } else if (s == "New1-2" && PlayerPrefs.GetInt("levelLock") < 5) {
                PlayerPrefs.SetInt("levelLock",5);
            } else if (s == "New1-3" && PlayerPrefs.GetInt("levelLock") < 6) {
                PlayerPrefs.SetInt("levelLock",6);
            } else if (s == "HubMine" && PlayerPrefs.GetInt("levelLock") < 7) {
                PlayerPrefs.SetInt("levelLock",7);
            } else if (s == "World2-0" && PlayerPrefs.GetInt("levelLock") < 8) {
                PlayerPrefs.SetInt("levelLock",8);
            } else if (s == "World2-1" && PlayerPrefs.GetInt("levelLock") < 9) {
                PlayerPrefs.SetInt("levelLock",9);
            } else if (s == "Boss_scene" && PlayerPrefs.GetInt("levelLock") < 10) {
                PlayerPrefs.SetInt("levelLock",10);
            }
            SceneTransitioner st = FindObjectOfType<SceneTransitioner>();
            if (st) st.LoadScene(s);
            else SceneManager.LoadScene(s);
        }
    }
}
