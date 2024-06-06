using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanWeapon : GunWeapon
{
    protected HitScanWeaponScriptableObject hitGun;
    [SerializeField] private VFX hitVFX;

    protected override void Awake()
    {
        base.Awake();
        hitGun = (HitScanWeaponScriptableObject)gun;
    }

    protected override void FrameTick()
    {
        base.FrameTick();
        
        if (active)
        {
            if (!sfx.IsPlaying) sfx.Play();
        }
        else sfx.Stop();
    }

    public override bool Fire()
    {
        if (!base.Fire()) return false;

        HitScan();
        return true;
    }

    private void HitScan()
    {
        var didHit = Physics.Raycast(firePoint.position, fireDir, out RaycastHit hit, hitGun.maxDistance);

        if (!didHit) return;
        
        var vfxObject = Instantiate(hitVFX, hit.point, Quaternion.identity);
        vfxObject.PlayOnce();

        var damageHit = SearchForDamageable(hit.collider.gameObject, out bool found);
        if (!found) return;
        
        if (myCombat.FoundMyself(damageHit)) return;
        
        SendDamageInfo(damageHit, gun.damage);
    }
}
