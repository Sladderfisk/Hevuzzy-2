using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageable : BaseDamageable
{
    [SerializeField] private WorldSpaceText worldSpaceText;
    
    public override void TakeHit(HitInfo hitInfo)
    {
        int text = worldSpaceText.Instantiate(transform.position, 0.5f, 3);
        if (text > -1)worldSpaceText.ChangeText(text, hitInfo.damage.ToString());

        base.TakeHit(hitInfo);
    }
}
