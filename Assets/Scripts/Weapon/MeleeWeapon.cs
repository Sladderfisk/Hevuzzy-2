using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    private MeleeWeaponScriptableObject melWep;

    private void Awake()
    {
        melWep = (MeleeWeaponScriptableObject)wep;
    }

    public override bool Fire()
    {
        if (!base.Fire()) return false;

        
        
        return true;
    }

    private IEnumerable Swing()
    {
        var dur = 0.0f;
        
        //while(dur < )
        yield return new WaitForEndOfFrame();
    }
}
