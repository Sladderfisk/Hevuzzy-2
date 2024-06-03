using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Weapons/Hit Scan Weapon", fileName = "Hit Scan Weapon")]
public class HitScanWeaponScriptableObject : BaseWeaponScriptableObject
{
    public float spread;
    public float recoil;
    public float maxDistance;
}