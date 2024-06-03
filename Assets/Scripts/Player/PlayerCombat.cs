
using Unity;
using UnityEngine;

public class PlayerCombat : BaseCombat
{
    private PlayerMovement movement;

    protected override void Awake()
    {
        base.Awake();
        
        movement = GetComponent<PlayerMovement>();
    }
    
    protected override void FrameTick()
    {
        base.FrameTick();
        
        ChangeWeaponInput();

        if (CurrentWeapon.Weapon.canHoldDown)
        {
            if (Input.GetMouseButton(0)) Attack();
        }
        else if (Input.GetMouseButtonDown(0)) Attack();
    }

    protected override bool Attack()
    {
        if (!base.Attack()) return false;

        movement.LookAt(Camera.main.transform.forward + transform.position);
        
        return true;
    }

    private void ChangeWeaponInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchWeapon(3);
    }
}