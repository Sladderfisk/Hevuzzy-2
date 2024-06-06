using UnityEngine;

public class ProjectileWeapon : GunWeapon
{
    [SerializeField] private int totalCachedProjectiles;

    protected ProjectileWeaponScriptableObject projWep;

    protected override void Awake()
    {
        base.Awake();

        projWep = (ProjectileWeaponScriptableObject)gun;
    }

    protected override void FrameTick()
    {
        base.FrameTick();
        
        if (active) vfx.Stop();
    }

    public override bool Fire()
    {
        if (!base.Fire()) return false;

        vfx.Play();
        Shoot();
        
        return true;
    }

    protected virtual void Shoot()
    {
        var proj = Instantiate(projWep.projectile);
        proj.Init(fireDir, firePoint.position, 
            new HitInfo()
        {
            damage = projWep.damage,
            shooter = myCombat,
            weapon = this
        } );
    }

    public override void Rotate()
    {
        transform.LookAt(fireDir + firePoint.position);
    }
}

