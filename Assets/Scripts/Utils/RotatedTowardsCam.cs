using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatedTowardsCam : PauseInGame
{
    protected override void FrameTick()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
