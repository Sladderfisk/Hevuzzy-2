using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : CanPause
{
    [SerializeField] private CinemachineFreeLook cin;
    [SerializeField] private CinemachineCameraOffset offset;

    [SerializeField] private float switchTime;

    [SerializeField] private Vector3 startOffset;
    [SerializeField] private Vector3 combatOffset;

    private Camera cam;
    
    private BaseMovement.CombatState curState;
    private bool isSwitching;
    
    private void Awake()
    {
        LockCursor();
    }

    private void Start()
    {
        cam = Camera.main;
    }

    public void SwitchState(BaseMovement.CombatState state)
    {
        var oldState = curState;
        curState = state;
        
        if (!isSwitching) StartCoroutine(Switching(state));
        else if (state != oldState) StartCoroutine(Switching(state));
    }

    private IEnumerator Switching(BaseMovement.CombatState state)
    {
        isSwitching = true;
        
        var target = state == BaseMovement.CombatState.Passive ? startOffset : combatOffset;
        var currOffset = offset.m_Offset;
        while (Vector3.Distance(currOffset, target) > 0.1f)
        {
            if (state != curState) yield break;
            
            offset.m_Offset = Vector3.Lerp(offset.m_Offset, target, Time.deltaTime / switchTime);
            yield return new WaitForEndOfFrameUnit();
        }

        isSwitching = false;
    }

    public Vector3 GetDirectionToCenter()
    {
        var ray = cam.ScreenPointToRay(new Vector3(cam.pixelWidth, cam.pixelHeight, 0) / 2);
        var didHit = Physics.Raycast(ray, out var hit);
        return didHit ? hit.point : ray.direction * 10000;
    }

    public static void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
