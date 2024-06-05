
using Unity;
using UnityEngine;

public class PlayerCombat : BaseCombat
{
    private PlayerMovement movement;
    private PlayerCamera playerCam;

    protected override void Awake()
    {
        base.Awake();

        playerCam = FindObjectOfType<PlayerCamera>();
        movement = GetComponent<PlayerMovement>();
    }
    
    protected override void FrameTick()
    {
        base.FrameTick();
        
        ChangeWeaponInput();
        CurrentWeapon.Rotate();

        if (CurrentWeapon.Weapon.canHoldDown)
        {
            if (Input.GetMouseButton(0)) Attack();
        }
        else if (Input.GetMouseButtonDown(0)) Attack();
        
        if (!CurrentWeapon.Active) movement.DisableCombat();
        else movement.SetCombat(Camera.main.transform.forward);

    }

    public override Vector3 GetFireDirection()
    {
        var camDir = playerCam.GetDirectionToCenter();
        return camDir;
    }

    protected override bool Attack()
    {
        if (!base.Attack()) return false;

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