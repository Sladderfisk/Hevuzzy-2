
using Unity;
using UnityEngine;

public class PlayerCombat : BaseCombat
{
    protected override void FrameTick()
    {
        base.FrameTick();

        if (Input.GetMouseButton(0)) Attack();
    }
}