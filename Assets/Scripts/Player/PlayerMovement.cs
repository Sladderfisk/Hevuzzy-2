using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerMovement : BaseMovement
{
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private PlayerCamera cam;
    
    private Vector3 inputDirection;

    public void SetCombat(Vector3 dir)
    {
        cam.SwitchState(CombatState.Combat);
        
        dir = Vector3.Scale(dir, new(1, 0, 1)).normalized;
        combatTargetDirection = dir;
        currentCombatState = CombatState.Combat;
    }

    public void DisableCombat()
    {
        cam.SwitchState(CombatState.Passive);

        currentCombatState = CombatState.Passive;
    }
    
    protected override void FrameTick()
    {
        inputDirection = new(
            Input.GetAxisRaw("Horizontal"),
            0.0f,
            Input.GetAxisRaw("Vertical")
            );
        inputDirection = inputDirection.normalized;

        SetState();
        PlayAnimation();
        
        if (currentCombatState == CombatState.Combat) Rotate();
    }

    private void PlayAnimation()
    {
        switch (CurrentJumpingState)
        {
            case JumpingState.IdleJump:
                playerAnimation.SetBool(PlayerAnimation.States.Jumping, true);
                break;
            
            case JumpingState.RunJump:
                playerAnimation.SetBool(PlayerAnimation.States.Jumping, true);
                break;
            
            case JumpingState.Air:
                playerAnimation.SetBool(PlayerAnimation.States.Jumping, false);
                playerAnimation.SetBool(PlayerAnimation.States.Falling, true);
                break;
            
            case JumpingState.Grounded:
                
                playerAnimation.SetBool(PlayerAnimation.States.Falling, false);
                playerAnimation.SetBool(PlayerAnimation.States.Jumping, false);
                switch (CurrentMovementState)
                {
                    case MovementState.Idle:
                        playerAnimation.SetBool(PlayerAnimation.States.Running, false);
                        playerAnimation.SetBool(PlayerAnimation.States.Walking, false);
                        break;
                    
                    case MovementState.Walking:
                        playerAnimation.SetBool(PlayerAnimation.States.Running, false);
                        playerAnimation.SetBool(PlayerAnimation.States.Walking, true);
                        break;
                    
                    case MovementState.Running:
                        playerAnimation.SetBool(PlayerAnimation.States.Running, true);
                        break;
                    
                    case MovementState.Strafing:
                        playerAnimation.SetBool(PlayerAnimation.States.Running, false);
                        playerAnimation.SetBool(PlayerAnimation.States.Walking, true);
                        break;
                }
                break;
        }
    }

    private void SetState()
    {
        if (inputDirection == Vector3.zero) currentMovementState = MovementState.Idle;
        else
        {
            if (Input.GetKey(KeyCode.LeftShift)) currentMovementState = MovementState.Running;
            else currentMovementState = MovementState.Walking;

            if (currentCombatState == CombatState.Combat) currentMovementState = MovementState.Strafing;
        }

        SetJumpState();
    }

    private void SetJumpState()
    {
        if (currentJumpingState != JumpingState.Grounded) return;
        if (currentMovementState == MovementState.Strafing) return;

        if (!Input.GetKeyDown(KeyCode.Space)) return;
        switch (currentMovementState)
        {
            case MovementState.Idle:
                currentJumpingState = JumpingState.IdleJump;
                break;
                
            case MovementState.Walking:
                currentJumpingState = JumpingState.RunJump;
                break;
                
            case MovementState.Running:
                currentJumpingState = JumpingState.RunJump;
                break;
        }
    }

    protected override void Strafing()
    {
        strafeDir = mCamera.transform.forward * inputDirection.z + mCamera.transform.right * inputDirection.x;
        base.Strafing();
    }

    protected override void Rotate()
    {
        targetDirection = mCamera.transform.forward * inputDirection.z + mCamera.transform.right * inputDirection.x;
        targetDirection = Vector3.Scale(targetDirection, new(1.0f, 0.0f, 1.0f));
        
        base.Rotate();
    }

    

    private void OnDrawGizmos()
    {
        float targetDirectionSphereRadius = 0.1f;

        Gizmos.color = Color.black;
        var pos = targetDirection + new Vector3(transform.position.x, 1, transform.position.z);
        Gizmos.DrawWireSphere(pos, targetDirectionSphereRadius);
    }
}
