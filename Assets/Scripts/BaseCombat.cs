using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseCombat))]
public abstract class BaseCombat : CanPause
{
    [SerializeField] private WeaponBase[] weapons;
    
    private WeaponBase currentWeapon;

    public WeaponBase CurrentWeapon => currentWeapon;

    private void Awake()
    {
        foreach (var weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        
        SwitchWeapon(0);
    }

    protected virtual bool Attack()
    {
        return currentWeapon.Fire();
    }

    private void SwitchWeapon(int i)
    {
        if (currentWeapon != null) currentWeapon.gameObject.SetActive(false);
        currentWeapon = weapons[i];
        currentWeapon.SetMyCombat(this);
        
        currentWeapon.gameObject.SetActive(true);
    }
}
