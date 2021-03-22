using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level_behavior : MonoBehaviour
{
    public GameObject levelMenu;

    void OnTriggerEnter(Collider coll) {
        if (levelMenu != null)
            levelMenu.SetActive(true);
    }
    void OnTriggerExit(Collider coll) {
        if (levelMenu != null)
            levelMenu.SetActive(false);
    }
}
