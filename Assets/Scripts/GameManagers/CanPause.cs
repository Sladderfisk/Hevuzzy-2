using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPause : MonoBehaviour
{
    private static GameState currentGameState = GameState.Active;

    public static GameState CurrentGameState
    {
        get => currentGameState;
        set => currentGameState = value;
    }

    private void Update()
    {
        IgnorePause();
        
        switch (currentGameState)
        {
            case GameState.Active:
                FrameTick();
                break;
        }
    }

    /// <summary>
    /// A version of Update that can't be paused.
    /// </summary>
    protected virtual void IgnorePause()
    {
        
    }
    
    /// <summary>
    /// A version of Update that can be paused.
    /// </summary>
    protected virtual void FrameTick()
    {
        
    }

    private void LateUpdate()
    {
        switch (currentGameState)
        {
            case GameState.Active:
                LateFrameTick();
                break;
        }
    }

    /// <summary>
    /// LateUpdate that can be paused
    /// </summary>
    protected virtual void LateFrameTick()
    {
        
    }
    
    private void FixedUpdate()
    {
        switch (currentGameState)
        {
            case GameState.Active:
                PhysTick();
                break;
        }
    }

    /// <summary>
    /// A version of FixedUpdate that can be paused.
    /// </summary>
    protected virtual void PhysTick()
    {
        
    }

    protected virtual void Destroy(GameObject obj)
    {
        obj.SetActive(false);
    }
}

public enum GameState
{
    Active,
    Paused
}
