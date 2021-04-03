using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxHoldingAnimator : MonoBehaviour
{
    Animator anim;
    private Subscription<MovementEvent> MovementEvent;
    private bool isWalking;
    private bool isJumping;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        MovementEvent = EventBus.Subscribe<MovementEvent>(MoveUpdate);
    }


    void MoveUpdate(MovementEvent e)
    {
        isWalking = e.isWalking;
        isJumping = e.isJumping;
       
    }

    // Update is called once per frame
    void Update()
    {
        if(isWalking && !isJumping)
        {
            anim.SetTrigger("isWalking");
        }
        else
        {
            anim.SetTrigger("notWalking");
        }
    }
}
