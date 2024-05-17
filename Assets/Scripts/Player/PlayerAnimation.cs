using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : PauseInGame
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetBool(Cond cond, bool val)
    {
        anim.SetBool(cond.ToString(), val);
    }
    
    public enum Cond
    {
        Walking,
        Running
    }
}
