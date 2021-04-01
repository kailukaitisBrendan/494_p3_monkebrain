using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Subscription<MovementEvent> MovementEvent;
    private bool isWalking;
    private bool isJumping;
    private bool isGrounded;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        MovementEvent = EventBus.Subscribe<MovementEvent>(MoveUpdate);
    }

    // Update is called once per frame
    void MoveUpdate(MovementEvent e)
    {
        isWalking = e.isWalking;
        isJumping = e.isJumping;
        isGrounded = e.isGrounded;
    }

    private void Update()
    {
        if (isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        if (isJumping)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

    }

  
}
