using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDamageable : CanPause
{
    [SerializeField] private int health;
    [SerializeField] private bool invincible;

    protected int currentHealth;

    public int CurrentHealth => currentHealth;

    public static Dictionary<int, BaseDamageable> AllDamageable { get; private set; }

    private void Awake()
    {
        if (AllDamageable == null) AllDamageable = new Dictionary<int, BaseDamageable>();
        AllDamageable.Add(gameObject.GetInstanceID(), this);
    }

    private void OnDisable()
    {
        AllDamageable.Remove(gameObject.GetInstanceID());
    }

    public virtual void TakeHit(HitInfo hitInfo)
    {
        if (!invincible) currentHealth -= hitInfo.damage;
        if (currentHealth < 1) Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}

public struct HitInfo
{
    public int damage;
    public BaseCombat shooter;
    public WeaponBase weapon;
}
