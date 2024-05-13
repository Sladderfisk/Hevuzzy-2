using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected float damage;

    public virtual void Fire()
    {
        
    }
}
