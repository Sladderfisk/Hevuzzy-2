using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Weapons/Melee Weapon", fileName = "Melee Weapon")]
public class MeleeWeaponScriptableObject : BaseWeaponScriptableObject
{
    public Combo[] combos;

    [Serializable]
    public struct Combo
    {
        public int damage;
        public AnimationClip ani;
        public float attackLenght;
    }
}
