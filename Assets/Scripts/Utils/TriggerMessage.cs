using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMessage : MonoBehaviour
{
    private List<Collider> collidersInMe;
    
    private Collider myCollider;

    public List<Collider> CollidersInMe => collidersInMe;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();

        collidersInMe = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        collidersInMe.Add(other);
    }
    
    private void OnTriggerExit(Collider other)
    {
        collidersInMe.Remove(other);
    }
}
