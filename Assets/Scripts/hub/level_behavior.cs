using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level_behavior : MonoBehaviour
{
    public GameObject levelMenu;

    void OnTriggerEnter(Collider coll)
    {
        if (!coll.CompareTag("Wagon")) return; 
        if (levelMenu != null)
            levelMenu.SetActive(true);
            Cursor.visible = true;
    }
    void OnTriggerExit(Collider coll)
    {
        if (!coll.CompareTag("Wagon")) return;
        if (levelMenu != null)
            levelMenu.SetActive(false);
            Cursor.visible = false;
    }
}
