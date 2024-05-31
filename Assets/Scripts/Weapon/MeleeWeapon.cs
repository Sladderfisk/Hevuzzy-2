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
    [SerializeField] private LayerMask layerMask;
    
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
        if (BaseDamageable.AllDamageable.ContainsKey(myCombat.gameObject.GetInstanceID())) 
            objectsHit.Add(BaseDamageable.AllDamageable[myCombat.gameObject.GetInstanceID()]);

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
            //Collider[] hits = new Collider[] {};
            var hits = Physics.OverlapBox(col.transform.position + col.center, col.size / 2f, col.transform.rotation, layerMask.value, QueryTriggerInteraction.Collide);
            var hitCount = hits.Length;

            var objs = new GameObject[hitCount];

            for (int i = 0; i < hitCount; i++)
            {
                objs[i] = hits[i].gameObject;
            } 

            var damageableHits = SearchForDamageable(objs);

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
