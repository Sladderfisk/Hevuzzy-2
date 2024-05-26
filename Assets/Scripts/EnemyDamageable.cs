using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageable : BaseDamageable
{
    [SerializeField] private WorldSpaceText DamageText;
    
    public override void TakeHit(HitInfo hitInfo)
    {
        base.TakeHit(hitInfo);
        
        
    }
}
