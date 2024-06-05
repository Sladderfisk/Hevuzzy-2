using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    [SerializeField] protected int startAmmo;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Animator animator;
    [SerializeField] protected VFX vfx;

    [SerializeField] protected GameObject temp;
    
    protected GunWeaponScriptableObject gun;

    protected Quaternion startRot;
    protected Vector3 fireDir;

    protected int currentAmmoAmount;
    protected int currentClipSize;

    protected virtual void Awake()
    {
        startRot = transform.localRotation;
        gun = (GunWeaponScriptableObject)wep;
    }

    protected override void FrameTick()
    {
        base.FrameTick();
        fireDir = (myCombat.GetFireDirection() - firePoint.position).normalized;
        animator.SetBool(Cases.Shooting.ToString(), active);
        if (!active) vfx.Stop();
    }
    
    protected override void SetActive()
    {
        
    }

    public virtual void Reload()
    {
        int amountToReloadWith = gun.clipSize - currentClipSize;
        if (amountToReloadWith > 1) return;
        if (currentAmmoAmount > 1) return;
        
        if (currentAmmoAmount > amountToReloadWith)
        {
            currentClipSize += amountToReloadWith;
            currentAmmoAmount -= amountToReloadWith;
        }
        else
        {
            currentClipSize += currentAmmoAmount;
            currentAmmoAmount = 0;
        }
    }

    protected override void LateFrameTick()
    {
        if (timeSinceLastAttack > timeToDeactivate) active = false;
    }

    public override bool Fire()
    {
        if (!base.Fire()) return false;

        active = true;
        vfx.Play();
        
        return true;
    }

    public override void Rotate()
    {
        if (!active) transform.localRotation = startRot;
        else
        {
            transform.rotation = Quaternion.LookRotation(fireDir) * new Quaternion(0.0f, -1.0f, 0.0f, 1.0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(firePoint.position, fireDir * 100);
    }

    private enum Cases
    {
        Shooting
    }
}
