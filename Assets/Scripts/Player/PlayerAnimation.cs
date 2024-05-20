using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : CanPause
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetTrigger(States states)
    {
        anim.SetTrigger(states.ToString());
    }

    public void SetBool(States states, bool val)
    {
        anim.SetBool(states.ToString(), val);
    }
    
    public enum States
    {
        Walking,
        Running,
        Idle,
        Jumping,
        Falling
    }
}
