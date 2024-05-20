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

    protected virtual void IgnorePause()
    {
        
    }
    
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

    protected virtual void PhysTick()
    {
        
    }
}

public enum GameState
{
    Active,
    Paused
}
