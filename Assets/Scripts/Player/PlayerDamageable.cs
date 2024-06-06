using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamageable : BaseDamageable
{
    [SerializeField] private PlayerUI ui;

    protected override void Awake()
    {
        base.Awake();
        
        ui.SetHealth(currentHealth);
    }
    
    public override void TakeHit(HitInfo hitInfo)
    {
        base.TakeHit(hitInfo);
        
        ui.SetHealth(currentHealth);
    }

    protected override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
