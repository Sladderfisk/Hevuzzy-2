using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerMovement : BaseMovement
{
    [SerializeField] private float combatRotationSpeed;
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private PlayerCamera cam;
    
    private Vector3 inputDirection;
    private Vector3 combatTargetDirection;
    private Vector3 targetDirection;
    private float currentMovementSpeed;

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

    protected override void Idle()
    {
        currentMovementSpeed = Accelerate(currentMovementSpeed, idle);
        velocity = forward * currentMovementSpeed;
    }

    protected override void Walking()
    {
        currentMovementSpeed = Accelerate(currentMovementSpeed, walking);
        velocity = forward * currentMovementSpeed;
    }

    protected override void Running()
    {
        currentMovementSpeed = Accelerate(currentMovementSpeed, running);
        velocity = forward * currentMovementSpeed;
    }

    protected override void Strafing()
    {
        currentMovementSpeed = Accelerate(currentMovementSpeed, strafing);
        var strafeDir = mCamera.transform.forward * inputDirection.z + mCamera.transform.right * inputDirection.x;
        velocity = strafeDir * currentMovementSpeed;
    }

    private float Accelerate(float val, MovementTemplate mov)
    {
        return Mathf.Lerp(val, mov.speed, Time.fixedDeltaTime / mov.accelerationSpeed);
    }

    protected override void Rotate()
    {
        switch (currentCombatState)
        {
            case CombatState.Passive:
                if (currentMovementState == MovementState.Idle) return;
        
                targetDirection = mCamera.transform.forward * inputDirection.z + mCamera.transform.right * inputDirection.x;
                targetDirection = Vector3.Scale(targetDirection, new(1.0f, 0.0f, 1.0f));
                RotateTo(targetDirection, rotationSpeed);
                break;
            
            case CombatState.Combat:
                
                RotateTo(combatTargetDirection, combatRotationSpeed);
                break;
        }
    }

    private void RotateTo(Vector3 targetDir, float rotSpeed)
    {
        var cRot = myRb.rotation;
        var targetRot = Quaternion.LookRotation(targetDir);
        targetRot.eulerAngles -= new Vector3(0.0f, 90.0f, 0.0f);
        targetRot *= new Quaternion(0, 1, 0, 1);
        myRb.rotation = Quaternion.Lerp(cRot, targetRot, Time.fixedDeltaTime / rotSpeed);
    }

    private void OnDrawGizmos()
    {
        float targetDirectionSphereRadius = 0.1f;

        Gizmos.color = Color.black;
        var pos = targetDirection + new Vector3(transform.position.x, 1, transform.position.z);
        Gizmos.DrawWireSphere(pos, targetDirectionSphereRadius);
    }
}
