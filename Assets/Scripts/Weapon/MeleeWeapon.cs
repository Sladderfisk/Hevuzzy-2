using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [SerializeField] BoxCollider[] damageColliders;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private float timeToDeactivate;
    
    private MeleeWeaponScriptableObject melWep;
    private MeleeWeaponScriptableObject.Combo currentCombo;

    private bool isAttacking;
    private int currentComboIndex;

    private void Awake()
    {
        melWep = (MeleeWeaponScriptableObject)wep;
    }

    protected override void FrameTick()
    {
        base.FrameTick();

        if (timeSinceLastAttack > timeToDeactivate)
        {
            currentComboIndex = 0;
            myAnimator.SetBool(States.Active.ToString(), false);
        }
    }

    public override bool Fire()
    {
        if (!base.Fire()) return false;
        if (isAttacking) return false;
        
        currentCombo = melWep.combos[currentComboIndex];
        
        currentComboIndex++;
        if (currentComboIndex >= melWep.combos.Length) currentComboIndex = 0;

            myAnimator.SetTrigger(currentCombo.ani.name);
        StartCoroutine(Swing());
        
        return true;
    }

    private IEnumerator Swing()
    {
        isAttacking = true;
        
        var dur = 0.0f;
        var objectsHit = new List<BaseDamageable>();
        
        // An easy way to not damage itself.
        if (BaseDamageable.AllDamageable.ContainsKey(myCombat.gameObject)) 
            objectsHit.Add(BaseDamageable.AllDamageable[myCombat.gameObject]);

        while (dur < currentCombo.ani.length)
        {
            dur += Time.fixedTime;
            Damage(objectsHit);

            yield return new WaitForFixedUpdate();
        }

        myAnimator.SetBool(States.Active.ToString(), true);
        isAttacking = false;
    }

    private void Damage(List<BaseDamageable> objectsHit)
    {
        foreach (var col in damageColliders)
        {
            var hits = Physics.BoxCastAll(col.center, col.size, Vector3.up, col.transform.rotation);

            var damageableHits = SearchForDamageable(hits);

            foreach (var hit in damageableHits)
            {
                if (objectsHit.Contains(hit)) continue;
                
                SendDamageInfo(hit);
            }
        }
    }

    private enum States
    {
        Active,
    }
}
