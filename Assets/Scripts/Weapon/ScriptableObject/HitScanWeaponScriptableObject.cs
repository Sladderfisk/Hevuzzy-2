using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Weapons/Hit Scan Weapon", fileName = "Hit Scan Weapon")]
public class HitScanWeaponScriptableObject : GunWeaponScriptableObject
{
    public float spread;
    public float maxDistance;
}