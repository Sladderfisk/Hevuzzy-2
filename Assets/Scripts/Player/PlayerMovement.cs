using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerMovement : BaseMovement
{
    [SerializeField] private PlayerAnimation playerAnimation;
    
    private Vector3 inputDirection;
    private Vector3 targetDirection;
    private float currentMovementSpeed;

    public void LookAt(Vector3 dir)
    {
        dir = Vector3.Scale(dir, new(1, 0, 1));
        targetDirection = dir;
        transform.rotation = Quaternion.LookRotation(dir) * new Quaternion(0, 1, 0, 1);
        transform.eulerAngles -= new Vector3(0.0f, 90.0f, 0.0f);
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
        }

        SetJumpState();
    }

    private void SetJumpState()
    {
        if (currentJumpingState != JumpingState.Grounded) return;

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

    private float Accelerate(float val, MovementTemplate mov)
    {
        return Mathf.Lerp(val, mov.speed, Time.fixedDeltaTime / mov.accelerationSpeed);
    }

    protected override void Rotate()
    {
        if (currentMovementState == MovementState.Idle) return;
        
        targetDirection = mCamera.transform.forward * inputDirection.z + mCamera.transform.right * inputDirection.x;
        targetDirection = Vector3.Scale(targetDirection, new(1.0f, 0.0f, 1.0f));
        
        var cRot = myRb.rotation;
        var targetRot = Quaternion.LookRotation(targetDirection);
        targetRot.eulerAngles -= new Vector3(0.0f, 90.0f, 0.0f);
        targetRot *= new Quaternion(0, 1, 0, 1);
        myRb.rotation = Quaternion.Lerp(cRot, targetRot, Time.fixedDeltaTime / rotationSpeed);
    }

    private void OnDrawGizmos()
    {
        float targetDirectionSphereRadius = 0.1f;

        Gizmos.color = Color.black;
        var pos = targetDirection + new Vector3(transform.position.x, 1, transform.position.z);
        Gizmos.DrawWireSphere(pos, targetDirectionSphereRadius);
    }
}
