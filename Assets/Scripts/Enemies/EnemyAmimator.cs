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

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        EnemyState = EventBus.Subscribe<EnemyStateEvent>(EnemyUpdate);
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
        if (isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        if (isDistracted)
        {
            animator.SetBool("isDistracted", true);
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isDistracted", false);
        }
        if (isDazed)
        {
            animator.SetBool("isDazed", true);
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isDazed", false);
        }
        if (drawingGun)
        {
           // Debug.Log("stickemup");
            animator.SetBool("DrawingGun", true);
            animator.SetBool("isWalking", false);
        }
    }
}
