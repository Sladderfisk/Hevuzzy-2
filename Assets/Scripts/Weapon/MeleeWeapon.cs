using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [SerializeField] private Collider[] damageColliders;
    [SerializeField] private Animator myAnimator;

    private MeleeWeaponScriptableObject melWep;
    private MeleeWeaponScriptableObject.Combo currentCombo;

    private List<TriggerMessage> damageCollidersInfo;

    private bool isAttacking;
    private int currentComboIndex;

    private void Awake()
    {
        melWep = (MeleeWeaponScriptableObject)wep;

        damageCollidersInfo = new List<TriggerMessage>();
        foreach (var collider in damageColliders)
        {
            damageCollidersInfo.Add(collider.gameObject.AddComponent<TriggerMessage>());
        }
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
        
        myAnimator.SetBool(States.Active.ToString(), true);
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

        while (dur < currentCombo.attackLenght)
        {
            dur += Time.fixedDeltaTime;
            Damage(objectsHit, currentCombo);

            yield return new WaitForFixedUpdate();
        }

        myAnimator.SetBool(States.Active.ToString(), true);
        isAttacking = false;
    }

    private void Damage(List<BaseDamageable> objectsHit, MeleeWeaponScriptableObject.Combo combo)
    {
        foreach (var col in damageColliders)
        {
            var hits = new List<Collider>();
            foreach (var info in damageCollidersInfo)
            {
                hits.AddRange(info.CollidersInMe);
            }
            var damageableHits = SearchForDamageable(hits);

            foreach (var hit in damageableHits)
            {
                if (objectsHit.Contains(hit)) continue;
                
                SendDamageInfo(hit, combo.damage);
                objectsHit.Add(hit);
            }
        }
    }

    private enum States
    {
        Active,
        Inactive
    }
}
