using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseCombat))]
public abstract class BaseCombat : PauseInGame
{
    private WeaponBase currentWeapon;

    public WeaponBase CurrentWeapon => currentWeapon;
}
