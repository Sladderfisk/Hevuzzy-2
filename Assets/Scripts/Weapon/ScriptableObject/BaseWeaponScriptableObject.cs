using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponScriptableObject : ScriptableObject
{
    [Tooltip("Ignore if there are other damage values")]public int damage;
    public float timeBetweenAttacks;
    public bool canHoldDown;
    public bool canCharge;
}
