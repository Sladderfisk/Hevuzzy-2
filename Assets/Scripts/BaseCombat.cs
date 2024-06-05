using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BaseCombat))]
public abstract class BaseCombat : CanPause
{
    [FormerlySerializedAs("weapons")] [SerializeField] private WeaponBase[] startWeapons;
    
    private WeaponBase currentWeapon;

    private List<WeaponBase> myWeapons;

    public WeaponBase CurrentWeapon => currentWeapon;

    public virtual Vector3 GetFireDirection()
    {
        return transform.forward;
    }

    protected virtual void Awake()
    {
        foreach (var weapon in startWeapons)
        {
            weapon.gameObject.SetActive(false);
        }

        myWeapons = new List<WeaponBase>();
        myWeapons.AddRange(startWeapons);
        
        SwitchWeapon(0);
    }

    protected virtual bool Attack()
    {
        return currentWeapon.Fire();
    }

    protected void SwitchWeapon(int i)
    {
        if (i >= myWeapons.Count) return;
        
        if (currentWeapon != null) currentWeapon.gameObject.SetActive(false);
        currentWeapon = myWeapons[i];
        currentWeapon.SetMyCombat(this);
        
        currentWeapon.gameObject.SetActive(true);
    }

    public bool FoundMyself(BaseDamageable hit)
    {
        if (!BaseDamageable.AllDamageable.ContainsKey(gameObject.GetInstanceID())) return false;

        return gameObject.GetInstanceID() == hit.gameObject.GetInstanceID();
    }
}
