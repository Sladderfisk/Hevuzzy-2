using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Weapons/Melee Weapon", fileName = "Melee Weapon")]
public class MeleeWeaponScriptableObject : BaseWeaponScriptableObject
{
    public Collider[] damageColliders;
    public Combo[] combos;

    [Serializable]
    public struct Combo
    {
        public float damage;
        public AnimationClip ani;
    }
}
