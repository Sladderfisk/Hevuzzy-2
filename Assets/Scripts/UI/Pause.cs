using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private Options optionsCanvas;

    private void Start()
    {
        pauseCanvas.gameObject.SetActive(false);
        optionsCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
    }

    public void PauseGame()
    {
        PlayerCamera.UnlockCursor();
        CanPause.CurrentGameState = GameState.Paused;
        Time.timeScale = 0;
        
        pauseCanvas.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        PlayerCamera.LockCursor();
        CanPause.CurrentGameState = GameState.Active;
        Time.timeScale = 1;
        
        pauseCanvas.gameObject.SetActive(false);
    }

    public void OpenOptions()
    {
        optionsCanvas.gameObject.SetActive(true);
        pauseCanvas.gameObject.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsCanvas.Save();
        optionsCanvas.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(true);
    }
}
