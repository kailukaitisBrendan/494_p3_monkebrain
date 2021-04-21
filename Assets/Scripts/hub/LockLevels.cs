using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockLevels : MonoBehaviour
{
    int num_levels = 0;
    void Start()
    {
        if (PlayerPrefs.GetInt("levelLock") > 0) {
            num_levels = PlayerPrefs.GetInt("levelLock");
        }
        // disable appropriate number of kids
        // number of active kids should be levelLock + 2
        // this code is from stack overflow
        for(int i = num_levels + 2; i< gameObject.transform.childCount; i++) {
            var child = gameObject.transform.GetChild(i).gameObject;
            if(child != null)
                child.SetActive(false);
        }
    }
}
