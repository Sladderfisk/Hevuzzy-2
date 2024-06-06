using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : BaseProjectile
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private AnimationCurve damageFalloff;
    [SerializeField] private SFX sfx;

    protected override void OnHit(Collider other)
    {
        var hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var hit in hits)
        {
            var myPos = transform.position;
            var otherPos = hit.transform.position;
            var distance = Vector3.Distance(myPos, otherPos);
            var dir = (otherPos - myPos).normalized;

            var falloff = damageFalloff.Evaluate(distance / explosionRadius);
            
            if (hit.attachedRigidbody != null) hit.attachedRigidbody.AddForce(dir * falloff *explosionForce);
            if (BaseDamageable.AllDamageable.ContainsKey(hit.gameObject.GetInstanceID()))
                Damage(BaseDamageable.AllDamageable[hit.gameObject.GetInstanceID()], falloff);
        }

        var sfxObject = Instantiate(sfx, transform.position, Quaternion.identity);
        sfxObject.DestroyAfterPlay();
        
        Destroy(gameObject);
    }

    private void Damage(BaseDamageable other, float falloff)
    {
        other.TakeHit(new HitInfo()
        {
            damage = (int)(myHitInfo.damage * falloff),
            shooter = myHitInfo.shooter,
            weapon = myHitInfo.weapon
        });
    }
}
