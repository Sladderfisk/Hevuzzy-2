using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Weapons/Projectile Weapon", fileName = "Projectile Weapon")]
public class ProjectileWeaponScriptableObject : GunWeaponScriptableObject
{
    public BaseProjectile projectile;
}
