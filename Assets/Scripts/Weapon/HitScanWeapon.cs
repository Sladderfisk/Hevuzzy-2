using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanWeapon : WeaponBase
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator animator;
    [SerializeField] private VFX vfx;
    
    private HitScanWeaponScriptableObject hitWep;
    
    private Vector3 fireDir;

    private void Awake()
    {
        hitWep = (HitScanWeaponScriptableObject)wep;
    }

    protected override void FrameTick()
    {
        base.FrameTick();
        fireDir = myCombat.GetFireDirection();
        animator.SetBool(Cases.Shooting.ToString(), active);
        if (!active) vfx.Stop();
    }

    public override bool Fire()
    {
        if (!base.Fire()) return false;

        HitScan();
        vfx.Play();
        
        return true;
    }

    private void HitScan()
    {
        var didHit = Physics.Raycast(firePoint.position, fireDir, out RaycastHit hit, hitWep.maxDistance);

        if (!didHit) return;

        var damageHit = SearchForDamageable(hit.collider.gameObject, out bool found);
        if (found) SendDamageInfo(damageHit, hitWep.damage);
    }
    
    private enum Cases
    {
        Shooting
    }
}
