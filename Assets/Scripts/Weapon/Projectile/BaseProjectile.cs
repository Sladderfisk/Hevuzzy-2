using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : CanPause
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask ignoreLayer;

    protected Rigidbody myRb;
    
    protected Vector3 direction;
    protected HitInfo myHitInfo;

    private void Awake()
    {
        myRb = GetComponent<Rigidbody>();
    }
    
    public void Init(Vector3 dir, Vector3 point, HitInfo hitInfo)
    {
        direction = dir;
        myHitInfo = hitInfo;

        transform.position = point;
        transform.LookAt(dir + point);
    }

    protected override void PhysTick()
    {
        myRb.velocity = direction * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");
        if (myHitInfo.shooter.gameObject == other.gameObject) return;
        Debug.Log("NotSame");
        if (Utils.CompareLayer(ignoreLayer, other.gameObject.layer)) return;
        Debug.Log("Layer");
        
        OnHit(other);
    }

    protected virtual void OnHit(Collider other)
    {
        
    }
}
