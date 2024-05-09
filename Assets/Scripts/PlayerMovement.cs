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
    
    private void Update()
    {
        inputDirection = new(
            Input.GetAxisRaw("Horizontal"),
            0.0f,
            Input.GetAxisRaw("Vertical")
            );
        inputDirection = inputDirection.normalized;

        SetState();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (currentJumpingState != JumpingState.Air) myRb.velocity += Vector3.up * jumpVel;
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
        playerAnimation.SetBool(PlayerAnimation.Cond.Walking, false);

        currentMovementSpeed = Accelerate(currentMovementSpeed, idle);
        myRb.velocity = forward * currentMovementSpeed;
    }

    protected override void Walking()
    {
        playerAnimation.SetBool(PlayerAnimation.Cond.Walking, true);
        
        currentMovementSpeed = Accelerate(currentMovementSpeed, walking);
        myRb.velocity = forward * currentMovementSpeed;
    }

    protected override void Running()
    {
        playerAnimation.SetBool(PlayerAnimation.Cond.Walking, false);

        currentMovementSpeed = Accelerate(currentMovementSpeed, running);
        myRb.velocity = forward * currentMovementSpeed;
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
