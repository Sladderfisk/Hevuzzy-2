using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    [SerializeField] protected MovementTemplate idle; 
    [SerializeField] protected MovementTemplate walking; 
    [SerializeField] protected MovementTemplate running;
    [Space] 
    [SerializeField] protected JumpTemplate idleJump;
    [SerializeField] protected JumpTemplate runJump;
    [SerializeField] protected float fallSpeed;
    [Space] 
    [SerializeField] protected float rotationSpeed;
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

    protected float jumpVel;
    
    protected MovementState currentMovementState;
    protected JumpingState currentJumpingState = JumpingState.Air;
    
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

    protected virtual void Awake()
    {
        myRb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        if (mCamera == null) mCamera = Camera.main;
    }

    protected virtual void FixedUpdate()
    {
        GroundCheck();
        Move();
        Jump();
        Rotate();

        if (hitGround) Debug.DrawRay(groundHitInfo.point, groundHitInfo.normal * 10);
        if (hitGround) forward = Vector3.Cross(groundHitInfo.normal, transform.right) * -1;
        else forward = transform.forward;
    }

    protected virtual void GroundCheck()
    {
        var hits = Physics.BoxCastAll(transform.position + Vector3.down * groundCheckLenght, 
            new Vector3(groundCheckRadius, groundCheckRadius, groundCheckRadius) * .5f, Vector3.up, 
            Quaternion.identity, 0.0f, groundLayer);
        
        hitGround = hits.Length > 0;
        if (!hitGround) return;
        
        groundHitInfo = Physics.RaycastAll(transform.position, Vector3.down, Mathf.Infinity, groundLayer)[0];
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
        }
    }
    
    protected virtual void Rotate()
    {
        
    }
    
    protected virtual void Idle()
    {
        myRb.velocity = transform.forward * idle.speed;
    }
    
    protected virtual void Walking()
    {
        
        myRb.velocity = transform.forward * walking.speed;
    }

    protected virtual void Running()
    {
        myRb.velocity = transform.forward * running.speed;
    }

    protected virtual void Jump()
    {
        //if (hitGround) currentJumpingState = JumpingState.Grounded;

        switch (currentJumpingState)
        {
            case JumpingState.Air:
                Falling();
                break;
            
            case JumpingState.IdleJump:
                Debug.Log("OvO");
                IdleJump();
                break;

            case JumpingState.RunJump:
                RunJump();
                break;
            
            default:
                jumpVel = 0;
                break;
        }
    }

    protected virtual void Falling()
    {
        myRb.velocity -= Vector3.down * fallSpeed;
        if (hitGround) currentJumpingState = JumpingState.Grounded;
    }

    protected virtual void IdleJump()
    {
        if (jumpVel > idleJump.jumpHeight - 0.1f)
        {
            Debug.Log("OVO");
            jumpVel = Mathf.Lerp(jumpVel, idleJump.jumpHeight, Time.fixedDeltaTime / idleJump.jumpSpeed);
        }
        else
        {
            currentJumpingState = JumpingState.Air;
        }
    }

    protected virtual void RunJump()
    {
        if (jumpVel > runJump.jumpHeight - 0.1f)
        {
            jumpVel = Mathf.Lerp(jumpVel, runJump.jumpHeight, Time.fixedDeltaTime / runJump.jumpSpeed);
        }
        else
        {
            currentJumpingState = JumpingState.Air;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = hitGround ? Color.green : Color.red;
        var pos = transform.position;
        //pos -= new Vector3(0.0f, groundCheckLenght, 0.0f);
        Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckLenght,
            new(groundCheckRadius, groundCheckRadius, groundCheckRadius));
    }

    public enum MovementState
    {
        Idle,
        Walking,
        Running,
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
