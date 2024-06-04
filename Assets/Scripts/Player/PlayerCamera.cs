using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : CanPause
{
    [SerializeField] private CinemachineCameraOffset offset;

    [SerializeField] private float switchTime;

    [SerializeField] private Vector3 startOffset;
    [SerializeField] private Vector3 combatOffset;
    
    private void Awake()
    {
        LockCursor();
    }

    public void SwitchState(BaseMovement.CombatState state)
    {
        switch (state)
        {
            case BaseMovement.CombatState.Combat:
                StartCoroutine(Switching(combatOffset));
                break;
            
            case BaseMovement.CombatState.Passive:
                StartCoroutine(Switching(startOffset));
                break;
        }
    }

    private IEnumerator Switching(Vector3 target)
    {
        var currOffset = offset.m_Offset;
        while (Vector3.Distance(currOffset, target) > 0.1f)
        {
            offset.m_Offset = Vector3.Lerp(offset.m_Offset, target, Time.deltaTime / switchTime);
            yield return new WaitForEndOfFrameUnit();
        }
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
