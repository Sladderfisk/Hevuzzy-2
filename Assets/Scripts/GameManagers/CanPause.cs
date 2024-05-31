using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPause : MonoBehaviour
{
    private GameState currentGameState = GameState.Active;

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
}

public enum GameState
{
    Active,
    Paused
}
