using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    private float nextActionTime = 6f;
    public float resetToIdle = 1.5f;
    public float period = 6f;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            // execute block of code here
            animator.SetTrigger("action");
        }
    }
}

