using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatedTowardsCam : CanPause
{
    protected override void FrameTick()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
