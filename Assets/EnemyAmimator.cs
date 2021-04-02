using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAmimator : MonoBehaviour
{
    private Subscription<EnemyStateEvent> EnemyState;

    public bool isWalking;
    public bool isDistracted;
    public bool isDazed;
    private bool drawingGun;
    
    private GameObject walking;
    private GameObject stickemup;
    private GameObject looking;
    private GameObject dazed;

    // Start is called before the first frame update
    void Start()
    {
        
        EnemyState = EventBus.Subscribe<EnemyStateEvent>(EnemyUpdate);
        walking = transform.Find("walking").gameObject;
        stickemup = transform.Find("stickemup").gameObject;
        looking = transform.Find("looking").gameObject;
        dazed = transform.Find("dazed").gameObject;

    }


    void EnemyUpdate(EnemyStateEvent e)
    {
        isWalking = e.isWalking;
        isDistracted = e.isDistracted;
        isDazed = e.isDazed;
        drawingGun = e.drawingGun;
    }
    // Update is called once per frame
    
    void Update()
    {
        
        
        if (isWalking && !isDistracted && !isDazed && !drawingGun)
        {
            walking.SetActive(true);
            looking.SetActive(false);
            dazed.SetActive(false);
            stickemup.SetActive(false);
        }
        

        if (isDistracted)
        {
            walking.SetActive(false);
            looking.SetActive(true);
            dazed.SetActive(false);
            stickemup.SetActive(false);
        }
        
        if (isDazed)
        {
            walking.SetActive(false);
            looking.SetActive(false);
            dazed.SetActive(true);
            stickemup.SetActive(false);
        }
        
        if (drawingGun)
        {
            walking.SetActive(false);
            looking.SetActive(false);
            dazed.SetActive(false);
            stickemup.SetActive(true);
        }
    }
}
