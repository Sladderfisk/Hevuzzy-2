using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyMovement : BaseMovement
{
    [SerializeField] private Transform[] patrolPositions;
    [SerializeField] private float searchLenght;
    [SerializeField] private float targetDistanceFromPlayer;

    protected static PlayerMovement PlayerMovement;
    protected static PlayerDamageable PlayerDamageable;

    protected EnemyStates currentEnemyState;

    private int currentPatrolIndex;
    private Vector3 currentPatrolPos;

    protected void Awake()
    {
        base.Awake();

        currentPatrolIndex = 0;
        if (patrolPositions.Length > 0) currentPatrolPos = patrolPositions[currentPatrolIndex].position;
        else currentPatrolPos = transform.position;

        if (PlayerMovement == null) PlayerMovement = FindObjectOfType<PlayerMovement>();
        if (PlayerDamageable == null) PlayerDamageable = FindObjectOfType<PlayerDamageable>();
    }

    protected override void FrameTick()
    {
        DoEnemyStuff();
    }

    private void DoEnemyStuff()
    {
        switch (currentEnemyState)
        {
            case EnemyStates.Patrolling:
                currentMovementState = MovementState.Walking;
                currentCombatState = CombatState.Passive;

                Patrolling();
                SearchForPlayer();
                break;
            
            case EnemyStates.Chasing:
                currentMovementState = MovementState.Running;
                currentCombatState = CombatState.Passive;

                ChasePlayer();
                break;
            case EnemyStates.Attacking:
                currentMovementState = MovementState.Strafing;
                currentCombatState = CombatState.Combat;

                combatTargetDirection = PlayerMovement.transform.position - transform.position;
                break;
        }
    }

    protected virtual void Patrolling()
    {
        if (patrolPositions.Length < 1)
        {
            currentMovementState = MovementState.Idle;
            return;
        }
        
        if (Vector3.Distance(transform.position, currentPatrolPos) < walking.speed)
        {
            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPositions.Length) currentPatrolIndex = 0;
            currentPatrolPos = patrolPositions[currentPatrolIndex].position;
        }

        targetDirection = currentPatrolPos - transform.position;
        targetDirection = Vector3.Scale(targetDirection, new(1.0f, 0.0f, 1.0f));
    }

    protected virtual void SearchForPlayer()
    {
        var distanceFromPlayer = Vector3.Distance(transform.position, PlayerMovement.transform.position);
        if (distanceFromPlayer > searchLenght) return;
        
        // Found player
        currentEnemyState = EnemyStates.Chasing;
    }

    protected virtual void ChasePlayer()
    {
        targetDirection = (PlayerMovement.transform.position - transform.position).normalized;
        targetDirection = Vector3.Scale(targetDirection, new(1.0f, 0.0f, 1.0f));

        var distanceFromPlayer = Vector3.Distance(transform.position, PlayerMovement.transform.position);
        if (distanceFromPlayer < targetDistanceFromPlayer) currentEnemyState = EnemyStates.Attacking;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        Gizmos.color = Color.white;
        
        Gizmos.DrawWireSphere(transform.position, searchLenght);
    }

    public enum EnemyStates
    {
        Patrolling,
        Chasing,
        Attacking
    }
}
