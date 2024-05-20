using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDamageable : CanPause
{
    [SerializeField] private int health;

    public static Dictionary<GameObject, BaseDamageable> AllDamageable { get; private set; }

    private void Awake()
    {
        if (AllDamageable == null) AllDamageable = new Dictionary<GameObject, BaseDamageable>();
        AllDamageable.Add(gameObject, this);
    }

    private void OnDisable()
    {
        AllDamageable.Remove(gameObject);
    }

    public virtual void TakeHit(HitInfo hitInfo)
    {
        
    }
}

public struct HitInfo
{
    public int damage;
    public BaseCombat shooter;
    public WeaponBase weapon;
}
