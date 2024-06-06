using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : CanPause
{
    [SerializeField] protected MovementTemplate idle; 
    [SerializeField] protected MovementTemplate walking; 
    [SerializeField] protected MovementTemplate running;
    [SerializeField] protected MovementTemplate strafing;
    [Space] 
    [SerializeField] protected JumpTemplate idleJump;
    [SerializeField] protected JumpTemplate runJump;
    [SerializeField] protected MovementTemplate fallSpeed;
    [Space] 
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected float combatRotationSpeed;
    [Space]
    [SerializeField] private float groundCheckLenght;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [Space]

    protected static Camera mCamera;
    protected Rigidbody myRb;

    protected RaycastHit groundHitInfo;
    protected bool hitGround;
    protected Vector3 forward;
    protected Vector3 strafeDir;
    
    protected Vector3 targetDirection;
    protected Vector3 combatTargetDirection;

    protected float currentMovementSpeed;
    protected float velocityY;
    protected Vector3 velocity;

    private float startJumpHeight;
    
    protected MovementState currentMovementState;
    protected JumpingState currentJumpingState = JumpingState.Air;
    protected CombatState currentCombatState = CombatState.Passive;
    
    public MovementState CurrentMovementState
    {
        get => currentMovementState;
        set => currentMovementState = value;
    }

    public JumpingState CurrentJumpingState
    {
        get => currentJumpingState;
        set => currentJumpingState = value;
    }

    public CombatState CurrentCombatState
    {
        get => currentCombatState;
        set => currentCombatState = value;
    }

    protected virtual void Awake()
    {
        myRb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        if (mCamera == null) mCamera = Camera.main;
    }

    protected override void PhysTick()
    {
        GroundCheck();
        Move();
        Jump();
        Rotate();

        if (hitGround) Debug.DrawRay(groundHitInfo.point, groundHitInfo.normal * 10);
        if (hitGround) forward = Vector3.Cross(groundHitInfo.normal, transform.right) * -1;
        else forward = transform.forward;

        myRb.velocity = new(velocity.x, velocity.y + velocityY, velocity.z);
    }

    protected virtual void GroundCheck()
    {
        var hits = Physics.BoxCastAll(transform.position + Vector3.down * groundCheckLenght, 
            new Vector3(groundCheckRadius, groundCheckRadius, groundCheckRadius) * .5f, Vector3.up, 
            Quaternion.identity, 0.0f, groundLayer);
        
        hitGround = hits.Length > 0;
        if (!hitGround) return;

        startJumpHeight = transform.position.y;
        groundHitInfo = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, Mathf.Infinity, groundLayer)[0];
    }

    protected virtual void Move()
    {
        switch (currentMovementState)
        {
            case MovementState.Idle:
                Idle();
                break;
            
            case MovementState.Walking:
                Walking();
                break;
            
            case MovementState.Running:
                Running();
                break;
            
            case MovementState.Strafing:
                Strafing();
                break;
        }
    }
    
    protected virtual void Rotate()
    {
        switch (currentCombatState)
        {
            case CombatState.Passive:
                if (currentMovementState == MovementState.Idle) return;
                RotateTo(targetDirection, rotationSpeed);
                break;
            
            case CombatState.Combat:
                
                RotateTo(combatTargetDirection, combatRotationSpeed);
                break;
        }
    }
    
    protected void RotateTo(Vector3 targetDir, float rotSpeed)
    {
        var cRot = myRb.rotation;
        var targetRot = Quaternion.LookRotation(targetDir);
        targetRot.eulerAngles -= new Vector3(0.0f, 90.0f, 0.0f);
        targetRot *= new Quaternion(0, 1, 0, 1);
        myRb.rotation = Quaternion.Lerp(cRot, targetRot, Time.fixedDeltaTime / rotSpeed);
    }
    
    protected virtual void Idle()
    {
        currentMovementSpeed = Accelerate(currentMovementSpeed, idle);
        velocity = forward * currentMovementSpeed;
    }

    protected virtual void Walking()
    {
        currentMovementSpeed = Accelerate(currentMovementSpeed, walking);
        velocity = forward * currentMovementSpeed;
    }

    protected virtual void Running()
    {
        currentMovementSpeed = Accelerate(currentMovementSpeed, running);
        velocity = forward * currentMovementSpeed;
    }

    protected virtual void Strafing()
    {
        currentMovementSpeed = Accelerate(currentMovementSpeed, strafing);
        velocity = strafeDir * currentMovementSpeed;
    }

    private float Accelerate(float val, MovementTemplate mov)
    {
        return Mathf.Lerp(val, mov.speed, Time.fixedDeltaTime / mov.accelerationSpeed);
    }

    protected virtual void Jump()
    {
        switch (currentJumpingState)
        {
            case JumpingState.Air:
                Falling();
                break;
            
            case JumpingState.IdleJump:
                IdleJump();
                break;

            case JumpingState.RunJump:
                RunJump();
                break;
            
            default:
                velocityY = 0;
                break;
        }
    }

    protected virtual void Falling()
    {
        velocityY = Mathf.Lerp(velocityY, -fallSpeed.speed, Time.fixedDeltaTime / fallSpeed.accelerationSpeed);
        
        if (hitGround) currentJumpingState = JumpingState.Grounded;
    }

    protected virtual void IdleJump()
    {
        Jump(idleJump);
    }

    protected virtual void RunJump()
    {
        Jump(runJump);
    }

    private void Jump(JumpTemplate a)
    {
        if (a.jumpHeight - a.jumpHeight / 5 > transform.position.y - startJumpHeight)
        {
            velocityY = Mathf.Lerp(a.jumpSpeed, 1f, (transform.position.y - startJumpHeight) / a.jumpHeight);
        }
        else
        {
            currentJumpingState = JumpingState.Air;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = hitGround ? Color.green : Color.red;
        var pos = transform.position;

        Gizmos.DrawWireCube(pos + Vector3.down * groundCheckLenght,
            new(groundCheckRadius, groundCheckRadius, groundCheckRadius));
    }

    public enum CombatState
    {
        Passive,
        Combat
    }

    public enum MovementState
    {
        Idle,
        Walking,
        Running,
        Strafing
    }

    public enum JumpingState
    {
        Grounded,
        Air,
        IdleJump,
        RunJump
    }

    [System.Serializable]
    public struct MovementTemplate
    {
        public float speed;
        public float accelerationSpeed;
    }

    [Serializable]
    public struct JumpTemplate
    {
        public float jumpHeight;
        public float jumpSpeed;
    }
}
