using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBoxTriggerEnter : MonoBehaviour
{
    private bool triggeredOnce = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 17 && !triggeredOnce)
        {
            triggeredOnce = true;
            EventBus.Publish<LevelClearEvent>(new LevelClearEvent());
            
        }
        if(other.gameObject.layer == 11)
        {
            
            Transform gp = other.gameObject.transform.Find("ItemSlot").Find("Golden Package");
            if (gp == null) {
                gp = other.gameObject.transform.Find("ItemSlot").Find("Golden Package (1)");
            }
            if (gp != null && !triggeredOnce)
            {
                
                triggeredOnce = true;
                EventBus.Publish<LevelClearEvent>(new LevelClearEvent());
            }
        }
    }
}
