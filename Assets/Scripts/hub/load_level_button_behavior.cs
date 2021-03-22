using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class load_level_button_behavior : MonoBehaviour
{
    public string s = "Main_Menu";

    public void OnButtonPress(){
        SceneManager.LoadScene(s);
    }
}
