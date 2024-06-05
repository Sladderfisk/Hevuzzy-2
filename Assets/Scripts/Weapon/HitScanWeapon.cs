using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanWeapon : WeaponBase
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator animator;
    [SerializeField] private VFX vfx;

    [SerializeField] private GameObject temp;
    
    private HitScanWeaponScriptableObject hitWep;

    private Quaternion startRot;
    
    private Vector3 fireDir;
    private bool firedThisFrame;

    private void Awake()
    {
        startRot = transform.localRotation;
        hitWep = (HitScanWeaponScriptableObject)wep;
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

    protected override void LateFrameTick()
    {
        if (timeSinceLastAttack > timeToDeactivate) active = false;
    }

    public override bool Fire()
    {
        if (!base.Fire()) return false;

        active = true;
        HitScan();
        vfx.Play();
        
        return true;
    }

    private void HitScan()
    {
        var didHit = Physics.Raycast(firePoint.position, fireDir, out RaycastHit hit, hitWep.maxDistance);

        if (!didHit) return;
        Instantiate(temp, hit.point, Quaternion.identity);

        var damageHit = SearchForDamageable(hit.collider.gameObject, out bool found);
        if (!found) return;
        
        if (myCombat.FoundMyself(damageHit)) return;
        SendDamageInfo(damageHit, hitWep.damage);
    }

    public override void Rotate()
    {
        if (!active) transform.localRotation = startRot;
        else
        {
            transform.rotation = Quaternion.LookRotation(fireDir) * new Quaternion(0.0f, -1.0f, 0.0f, 1.0f);
            //transform.eulerAngles -= new Vector3(0.0f, 180.0f, 0.0f);
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
