using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBoxTriggerEnter : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 17)
            EventBus.Publish<LevelClearEvent>(new LevelClearEvent());
        if(other.gameObject.layer == 11)
        {
            
            Transform gp = other.gameObject.transform.Find("ItemSlot").Find("Golden Package");
            if (gp != null)
            {
                EventBus.Publish<LevelClearEvent>(new LevelClearEvent());
            }
        }
    }
}
