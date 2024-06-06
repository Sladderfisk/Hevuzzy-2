using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyCombat : BaseCombat
{
    [SerializeField] private float playerHeight;
    private GroundEnemyMovement movement;
    
    protected static PlayerDamageable player;

    protected override void Awake()
    {
        base.Awake();

        movement = GetComponent<GroundEnemyMovement>();

        if (player == null) player = FindObjectOfType<PlayerDamageable>();
    }

    protected override void FrameTick()
    {
        if (movement.CurrentCombatState == BaseMovement.CombatState.Passive) return;
        base.FrameTick();
        
        CurrentWeapon.Rotate();
        Attack();
    }

    public override Vector3 GetFireDirection()
    {
        return player.transform.position + Vector3.up * playerHeight;
    }
}
