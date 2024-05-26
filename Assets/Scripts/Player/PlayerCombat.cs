
using Unity;
using UnityEngine;

public class PlayerCombat : BaseCombat
{
    protected override void FrameTick()
    {
        base.FrameTick();

        if (CurrentWeapon.Weapon.canHoldDown)
        {
            if (Input.GetMouseButton(0)) CurrentWeapon.Fire();
        }
        else if (Input.GetMouseButtonDown(0)) CurrentWeapon.Fire();
    }
}