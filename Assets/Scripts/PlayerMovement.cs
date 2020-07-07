using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Joystick joystick;
    private Animator animator;
    public Vector3 change, idleChange;

    [SerializeField] private float moveSpeed;

    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        idleChange = Vector3.zero;
        change.x = joystick.Horizontal;
        change.y = joystick.Vertical;

        UpdateAnimationAndMove();
    }

    public void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            change.x = Mathf.Round(change.x);
            change.y = Mathf.Round(change.y);
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);
        }
    }

    void MoveCharacter()
    {
        //totalspeed is base speed + skill effect and others
        change.Normalize();
        rb.MovePosition(transform.position + change * moveSpeed * Time.deltaTime);
    }

}