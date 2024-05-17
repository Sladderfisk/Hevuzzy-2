using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDamageable : PauseInGame
{
    [SerializeField] private int health;
    
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
