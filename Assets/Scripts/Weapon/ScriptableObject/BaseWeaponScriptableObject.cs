using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponScriptableObject : ScriptableObject
{
    public float damage;
    public float timeBetweenAttacks;
    public bool canHoldDown;
}
