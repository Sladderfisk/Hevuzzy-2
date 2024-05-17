using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : PauseInGame
{
    [SerializeField] protected BaseWeaponScriptableObject wep;

    protected BaseCombat myCombat;

    private float timeSinceLastHit = 999.0f;

    public virtual bool Fire()
    {
        if (!CanFire()) return false;
        
        return true;
    }

    protected bool CanFire()
    {
        bool val = timeSinceLastHit > wep.timeBetweenAttacks;
        if (val) timeSinceLastHit = 0;
        return val;
    }

    protected void SendDamageInfo(BaseDamageable hit)
    {
        HitInfo hitInfo = new HitInfo()
        {
            damage = wep.damage,
            shooter = myCombat,
            weapon = this
        };
        
        hit.TakeHit(hitInfo);
    }

    protected override void FrameTick()
    {
        timeSinceLastHit += Time.deltaTime;
    }
}
