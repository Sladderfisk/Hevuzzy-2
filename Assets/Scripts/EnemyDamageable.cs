using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageable : BaseDamageable
{
    [SerializeField] private WorldSpaceText worldSpaceText;
    
    public override void TakeHit(HitInfo hitInfo)
    {
        worldSpaceText.Instantiate(transform.position, 0.5f, 3);
        Debug.Log("Hit " + hitInfo.damage);

        base.TakeHit(hitInfo);

    }
}
