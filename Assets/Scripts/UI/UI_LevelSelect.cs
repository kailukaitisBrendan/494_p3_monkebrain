using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelSelect : MonoBehaviour
{
    public GameObject level_menus;
    public GameObject main_menus;

    void Start() { }

    void Update() { }

    public void OnButtonPress(){
        level_menus.SetActive(true);
        main_menus.SetActive(false);
    }
}
